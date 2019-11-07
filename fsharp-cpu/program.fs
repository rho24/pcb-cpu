module fsharp_cpu.program

open domain
open execution

let printState state =
    printfn "State:"
    printfn "HALTED: %b" state.halted
    printfn "PC: %X" state.pc
    printfn "Registers:"
    printfn "%A" state.registers
    printfn "Memory:"
    for MemAddr(addr),value in state.memory |> Map.toList do
        printfn "0x%04X -> %d" addr value

[<EntryPoint>]
let main args =
    let initialState = createState []

    let program = Map.ofList [
        0x00us, LoadImm {|dReg = X5; immediate = 25s|}
        0x02us, LoadImm {|dReg = X6; immediate = -5s|}
        0x04us, Alu {|aluFunc = Add; dReg = X7; aReg = X5; bReg = X6|}
        0x06us, StoreMem {|dReg = X7; memAddr = MemAddr 0x80us|}
    ]

    let finalState = simpleRunToHalted program initialState

    printState finalState |> ignore
    0