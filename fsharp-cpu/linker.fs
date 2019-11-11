module fsharp_cpu.linker
open System
open System.Globalization
open Microsoft.VisualBasic
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
    |> Strings.Trim
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
    | _ -> (currentAddress + 2us), instructions @ [ (currentAddress, inst) ], labels

let mapReg = function
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
    
let mapMemAddr labels arg =
    MemAddr (match arg with
    | Match "^0x.*" _ -> UInt16.Parse (arg,NumberStyles.HexNumber)
    | UInt16 i        -> i
    | _               -> labels |> List.find (fun (a,l) -> l = arg) |> (fun (a,l) -> a))

let mapToInstruction labels (addr,inst) =
    match inst with
    | Op3 ("add",a1,a2,a3) -> addr,Alu {aluFunc=Add; dReg=mapReg a1; aReg=mapReg  a2; bReg=mapReg  a3;} 
    | Op1 ("jump",a1)      -> addr,Jump {memAddr=mapMemAddr labels a1}
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
