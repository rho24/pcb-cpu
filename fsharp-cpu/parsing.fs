module fsharp_cpu.parsing

open domain

let parseRegAddr = function
    | 0 -> X0
    | 1 -> X1
    | 2 -> X2
    | 3 -> X3
    | 4 -> X4
    | 5 -> X5
    | 6 -> X6
    | 7 -> X7
    | 8 -> X8
    | 9 -> X9
    | 10 -> X10
    | 11 -> X11
    | 12 -> X12
    | 13 -> X13
    | 14 -> X14
    | 15 -> X15
    | _  -> X0

let parseInstruction instructionHalf1 instructionHalf2 =
    let getRd instructionRaw =
        let i = instructionRaw &&& (int16 0b111100000) >>> 5
        parseRegAddr (int i)
        
    let opcode = instructionHalf1 &&& (int16 0b11111) |> int
    match opcode with
    | 0b00001 -> LoadImm {|dReg = getRd instructionHalf1; immediate = instructionHalf2|}
    | _  -> Unknown

let loadInstruction state =
    let instructionHalf1 = Map.tryFind (MemAddr state.pc) state.memory |> Option.defaultValue 0s
    let instructionHalf2 = Map.tryFind (MemAddr (state.pc + 1us)) state.memory |> Option.defaultValue 0s
    let instruction = parseInstruction instructionHalf1 instructionHalf2
    instruction
