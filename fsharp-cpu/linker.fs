module fsharp_cpu.linker
open System
open System.Globalization
open System.IO
open System.Text.RegularExpressions
open fsharp_cpu.domain


let (|Match|_|) pattern input =
    let m = Regex.Match(input, pattern) in
    if m.Success then Some (List.tail [ for g in m.Groups -> g.Value ]) else None

// create an active pattern
let (|UInt16|_|) (str:string) =
   match UInt16.TryParse(str) with
   | (true,int) -> Some(int)
   | _ -> None

type Ops =
    | Op0 of string
    | Op1 of string * string
    | Op2 of string * string * string
    | Op3 of string * string * string * string
    | Op4 of string * string * string * string * string
    | Label of string

let parseLine (line:string) =
    line
    |> fun (s:string) -> Regex.Replace(s, "#.*$", "")
    |> fun s -> s.Trim()
    |> fun s ->
        match s with
        | Match "^(\w+)\s+(\w+),\s*(\w+),\s*(\w+),\s*(\w+)" [op; a1; a2; a3; a4] -> Some (Op4 (op,a1,a2,a3,a4))
        | Match "^(\w+)\s+(\w+),\s*(\w+),\s*(\w+)" [op; a1; a2; a3] -> Some (Op3 (op,a1,a2,a3))
        | Match "^(\w+)\s+(\w+),\s*(\w+)" [op; a1; a2] -> Some (Op2 (op,a1,a2))
        | Match "^(\w+)\s+(\w+)" [op; a1] -> Some (Op1 (op,a1))
        | Match "^(\w+)" [op] -> Some (Op0 (op))
        | Match "^:(\w+)" [l] -> Some (Label l)
        | _ -> None

let expandPsuedo i = [i] 

let addAddresses (currentAddress:uint16, instructions: (uint16*Ops) list, labels: (uint16*string) list) inst =
    match inst with
    | Label l -> currentAddress, instructions, labels @ [ (currentAddress, l) ]
    | _ -> (currentAddress + 4us), instructions @ [ (currentAddress, inst) ], labels

let strToReg = function
    | "x0"  -> X0
    | "x1"  -> X1
    | "x2"  -> X2
    | "x3"  -> X3
    | "x4"  -> X4
    | "x5"  -> X5
    | "x6"  -> X6
    | "x7"  -> X7
    | "x9"  -> X9
    | "x8"  -> X8
    | "x10" -> X10
    | "x11" -> X11
    | "x12" -> X12
    | "x13" -> X13
    | "x14" -> X14
    | "x15" -> X15

let regToWord = function
    | X0 -> 0u
    | X1 -> 1u
    | X2 -> 2u
    | X3 -> 3u
    | X4 -> 4u
    | X5 -> 5u
    | X6 -> 6u
    | X7 -> 7u
    | X8 -> 8u
    | X9 -> 9u
    | X10 -> 10u
    | X11 -> 11u
    | X12 -> 12u
    | X13 -> 13u
    | X14 -> 14u
    | X15 -> 15u
    
let mapMemAddr labels arg =
    MemAddr (
        match arg with
        | Match "^0x.*" _ -> UInt16.Parse (arg,NumberStyles.HexNumber)
        | UInt16 i        -> i
        | _               -> labels |> List.find (fun (a,l) -> l = arg) |> (fun (a,l) -> a))

let mapToInstruction labels (addr,inst) =
    match inst with
    | Op3 ("add",a1,a2,a3) -> addr,Alu {aluFunc=Add; dReg=strToReg a1; aReg=strToReg  a2; bReg=strToReg  a3;} 
    | Op2 ("load",a1,a2)   -> addr,LoadImm {dReg=strToReg a1; immediate=Int16.Parse(a2)} 
    | Op3 ("brancheq",a1,a2,a3)   -> addr,BranchEqual {aReg=strToReg a1; bReg=strToReg a2;offset=Int16.Parse(a3)} 
    | Op1 ("jump",a1)      -> addr,Jump {memAddr=mapMemAddr labels a1}
    | Op0 ("halt")         -> addr,Halt
    | _                    -> addr,Unknown

let compileAndLink (program:Stream) =
    
    use sr = new StreamReader(program)
    let lines = seq { while not sr.EndOfStream do yield sr.ReadLine() }
    
    let (_, instructions, labels) =
        lines
        |> Seq.choose parseLine
        |> Seq.collect expandPsuedo
        |> Seq.fold addAddresses (0us, [], [])
        
    instructions
    |> List.map (mapToInstruction labels)

let assemble (instructions: Map<uint16, Instruction>) =
    let aluFuncToWord = function
        | Add     -> 0u
        | Sub     -> 1u
        | And     -> 2u
        | Or      -> 3u
        | Xor     -> 4u
        | ShiftL  -> 5u
        | ShiftR  -> 6u
        | Compare -> 7u

    let instructionToWord k = function
        | Halt -> 0u
        | LoadImm i -> (i.immediate |> uint32 <<< 12)
                        + (i.dReg |> regToWord <<< 7)
                        + 0b0000111u 
        | Alu i -> (i.bReg |> regToWord <<< 20)
                    + (i.bReg |> regToWord <<< 15)
                    + (i.dReg |> regToWord <<< 7)
                    + (i.aluFunc |> aluFuncToWord <<< 12)
                    + 0b0110011u
        | _ -> 0u
    
    let wordToBytes w =
        let mask = Byte.MaxValue |> uint32
        w &&& mask |> byte
        , w &&& (mask <<<  8) >>>  8 |> byte
        , w &&& (mask <<< 16) >>> 16 |> byte
        , w &&& (mask <<< 24) >>> 24 |> byte    
    
    let bytes =
        instructions
        |> Map.map instructionToWord
        |> Map.toList
        |> List.collect
               (fun (k, w) ->
                    let b0, b1, b2, b3 = wordToBytes w
                    [ k      , b1;
                      k + 1us, b0;
                      k + 2us, b3;
                      k + 3us, b2 ])
        |> Map.ofList

    let getByteOrDefault bytes a =
        bytes
        |> Map.tryFind (uint16 a)
        |> Option.defaultValue 0uy
        
    List.init (int UInt16.MaxValue) (getByteOrDefault bytes)
    |> List.toArray