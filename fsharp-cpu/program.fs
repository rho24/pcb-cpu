module fsharp_cpu.program

open System.IO
open System.Text
open System.Text
open System.Text
open System.Text.Unicode
open domain
open execution

let printState state =
    printfn "State:"
    printfn "HALTED: %b" state.halted
    printfn "PC: %04X" state.pc
    printfn "Registers:"
    printfn "%A" state.registers
    printfn "Memory:"
    for MemAddr(addr),value in state.memory |> Map.toList do
        printfn "0x%04X -> %d" addr value

[<EntryPoint>]
let main args =
    let p = """
    # comment
    load x1, 2
    load x2, 0
    load x3, 32
:loop
    add x2, x2, x1 # adding 2
    brancheq x2, x3, 4
    jump loop
    halt
"""
    let l = p |> Encoding.UTF8.GetBytes
              |> fun b -> new MemoryStream(b)
              |> linker.compileAndLink
              |> Map.ofList
    
    printfn "%A" l
    
    let initialState = createState []

    let program = Map.ofList [
        0x00us, LoadImm {dReg = X5; immediate = 25s}
        0x02us, LoadImm {dReg = X6; immediate = -5s}
        0x04us, Alu {aluFunc = Add; dReg = X5; aReg = X5; bReg = X6}
        0x06us, StoreMem {dReg = X5; memAddr = MemAddr 0x80us}
        0x08us, BranchEqual {aReg = X5; bReg = X0; offset = 4s}
        0x0aus, Jump {memAddr = MemAddr 0x04us}
        0x0cus, Halt
    ]

//    let finalState = simpleRunToHalted program initialState
    let finalState = simpleRunToHalted l initialState

    printState finalState |> ignore
    0