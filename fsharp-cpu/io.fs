module fsharp_cpu.io

open domain

let readMem memAddr memory =
    Map.tryFind memAddr memory |> Option.defaultValue 0s

let writeMem memAddr value memory =
    Map.add memAddr value memory
    
let readReg regAddr registers =
    match regAddr with
    | X0 -> 0s
    | _  -> Map.tryFind regAddr registers |> Option.defaultValue 0s 
    
let writeReg regAddr value registers =
    match regAddr with
    | X0 -> registers
    | _  -> Map.add regAddr value registers
