module fsharp_cpu.program

open domain
open execution

[<EntryPoint>]
let main args =
    
    let initialState = createState [
        int16 0b000100001
        15s
     ]
    
    let newState = executeTick initialState

    0