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
    let {pc = pc; registers = registers; memory = memory} = state
    let incrementedState = {state with pc = pc + 2us}
    match instruction with
    | LoadImm {dReg=dReg;immediate=immediate} -> {incrementedState with registers = writeReg dReg immediate registers}
    | Alu {aluFunc = aluFunc; dReg = dReg; aReg = aReg; bReg = bReg} ->
        let a = readReg aReg registers
        let b = readReg bReg registers
        let result = executeAlu aluFunc a b
        {incrementedState with registers = writeReg dReg result registers}
    | LoadMem {dReg = dReg; memAddr = memAddr} ->
        let value = readMem memAddr memory
        {incrementedState with registers = writeReg dReg value registers }
    | StoreMem {dReg = dReg; memAddr = memAddr} ->
        let value = readReg dReg registers
        {incrementedState with memory = writeMem memAddr value memory }
    | BranchEqual {aReg = aReg; bReg = dReg; offset = offset} ->
        let a = readReg aReg registers
        let b = readReg dReg registers
        let offsetPc = pc |> int16 |> ((+) offset) |> uint16
        if a = b then {state with pc = offsetPc } else incrementedState
    | BranchNotEqual {aReg = aReg; bReg = dReg; offset = offset} ->
        let a = readReg aReg registers
        let b = readReg dReg registers
        let offsetPc = pc |> int16 |> ((+) offset) |> uint16
        if a <> b then {state with pc = offsetPc } else incrementedState
    | Jump {memAddr = memAddr} ->
        let (MemAddr value) = memAddr
        {state with pc = value}
    | JumpOffset {offset = offset} ->
        let offsetPc = pc |> int16 |> ((+) offset) |> uint16
        {state with pc = offsetPc}
    | JumpAndLink {memAddr = memAddr} ->
        let (MemAddr value) = memAddr
        {state with pc = value; registers = writeReg X1 (int16 pc) registers}
    | JumpAndLinkOffset {offset = offset} ->
        let offsetPc = pc |> int16 |> ((+) offset) |> uint16
        {state with pc = offsetPc; registers = writeReg X1 (int16 pc) registers}
    | Unknown | Halt -> {state with halted = true}

let simpleExecuteTick program state =
    let instruction = Map.tryFind state.pc program |> Option.defaultValue Unknown
    executeInstruction instruction state

let rec simpleRunToHalted program state =
    let nextState = simpleExecuteTick program state
    match nextState.halted with
    | true -> state
    | false -> simpleRunToHalted program nextState