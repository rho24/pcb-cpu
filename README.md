# pcb-cpu

Project to create a cpu across multiple pcbs.



## Thoughts
* Risc-v inspired
* 16bit
* 



## Instructions
* Reg     - Load Immediate
* ALU ops - ADD, SUB, SHIFT L/R, AND, OR, XOR, Set Compare
* Mem ops - Load mem, Store mem
* PC ops  - Branch equal, Branch not equal, Jump, Jump Offset, Jump and Link, Jump and Link Offset, Ret


## Registries
* x0 - Zero
* x1 - Return address
* x2 - Stack pointer
* x3 - Function arg/return/temp
* x4 - Function arg/return/temp
* x5 - Function arg/temp
* x6 - Function arg/temp
* x7 - temp

## Instruction formats
                    F E D C B A 9 8 7 6 5 4 3 2 1 0
Load Imm            Imm-----------------Rd----Opcode
ALU                 Ra----Rb----  Func--Rd----Opcode
Mem                 Ra----Rofset        Rd----Opcode
Branch              Ra----              Rd----Opcode
Jump                Imm-----------------Rd----Opcode
Jump Offset         Imm-----------------Rd----Opcode
Jump Reg            Ra----              Rd----Opcode
