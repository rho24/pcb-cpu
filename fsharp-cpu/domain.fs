module fsharp_cpu.domain

type RegAddr =
    | X0
    | X1
    | X2
    | X3
    | X4
    | X5
    | X6
    | X7
    | X8
    | X9
    | X10
    | X11
    | X12
    | X13
    | X14
    | X15

type MemAddr = MemAddr of uint16

type AluFunc =
    | Add
    | Sub
    | And
    | Or
    | Xor
    | ShiftL
    | ShiftR
    | Compare

type Instruction =
    | Unknown
    | LoadImm of {| dReg:RegAddr; immediate:int16 |}
    | Alu of {| aluFunc:AluFunc; aReg:RegAddr; bReg:RegAddr; dReg:RegAddr |}
    | LoadMem of {| dReg:RegAddr; memAddr:MemAddr |}
    | StoreMem of {| dReg:RegAddr; memAddr:MemAddr |}
    | BranchEqual of {| dReg:RegAddr; immediate:int16 |}
    | BranchNotEqual of {| dReg:RegAddr; immediate:int16 |}
    | Jump of {| memAddr:MemAddr |}
    | JumpOffset of {| immediate:int16 |}
    | JumpAndLink of {| memAddr:MemAddr |}
    | JumpAndLinkOffset of {| immediate:int16 |}
    
type State = {
    registers: Map<RegAddr,int16>
    memory: Map<MemAddr,int16>
    pc: uint16
    halted: bool
}

let createState (program: int16 list) =
    {
        registers = [X0, 0s; X1, 0s; X2, 0s] |> Map.ofList
        memory = program |> List.mapi (fun i s -> MemAddr (uint16 i), s) |> Map.ofList
        pc = 0us
        halted = false
    }