module fsharp_cpu.execution

open domain
open io

let executeAlu aluFunc a b =
    match aluFunc with
    | Add       -> a + b
    | Sub       -> a - b
    | And       -> a &&& b
    | Or        -> a ||| b
    | Xor       -> a ^^^ b
    | ShiftL    -> a <<< 1
    | ShiftR    -> a >>> 1
    | Compare   -> if a = b then 0s elif a < b then -1s else 1s

let executeInstruction instruction state =
    let incrementedState = {state with pc = state.pc + 2us}
    match instruction with
    | LoadImm i -> {incrementedState with registers = writeReg i.dReg i.immediate state.registers}
    | Alu i ->
        let a = readReg i.aReg state.registers
        let b = readReg i.bReg state.registers
        let result = executeAlu i.aluFunc a b
        {incrementedState with registers = writeReg i.dReg result state.registers}
    | LoadMem i ->
        let value = readMem i.memAddr state.memory
        {incrementedState with registers = writeReg i.dReg value state.registers }
    | StoreMem i ->
        let value = readReg i.dReg state.registers
        {incrementedState with memory = writeMem i.memAddr value state.memory }
//    | BranchEqual of {| dReg:RegAddr; immediate:int16 |}
//    | BranchNotEqual of {| dReg:RegAddr; immediate:int16 |}
//    | Jump of {| memAddr:MemAddr |}
//    | JumpOffset of {| immediate:int16 |}
//    | JumpAndLink of {| memAddr:MemAddr |}
//    | JumpAndLinkOffset of {| immediate:int16 |}
    
    | _ -> {state with halted = true}

let simpleExecuteTick program state =
    let instruction = Map.tryFind state.pc program |> Option.defaultValue Unknown
    executeInstruction instruction state

let rec simpleRunToHalted program state =
    let nextState = simpleExecuteTick program state
    match nextState.halted with
    | true -> state
    | false -> simpleRunToHalted program nextState