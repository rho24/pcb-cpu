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

module instructions =
    type ImmInst = { dReg:RegAddr; immediate:int16 }
    type Alu = { aluFunc:AluFunc; aReg:RegAddr; bReg:RegAddr; dReg:RegAddr }
    type Mem = { dReg:RegAddr; memAddr:MemAddr }
    type Branch = { aReg:RegAddr; bReg:RegAddr; offset:int16 }
    type Jump = { memAddr:MemAddr }
    type JumpOffset = { offset:int16 }

type Instruction =
    | Unknown
    | Halt
    | LoadImm of instructions.ImmInst
    | Alu of instructions.Alu
    | LoadMem of instructions.Mem
    | StoreMem of instructions.Mem
    | BranchEqual of instructions.Branch
    | BranchNotEqual of instructions.Branch
    | Jump of instructions.Jump
    | JumpOffset of instructions.JumpOffset
    | JumpAndLink of instructions.Jump
    | JumpAndLinkOffset of instructions.JumpOffset
    
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