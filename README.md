# pcb-cpu

Project to create a cpu across multiple pcbs.


## Thoughts
* Risc-v inspired
* 16bit


## Useful links
* https://rv8.io/isa.html
* https://www.mouser.co.uk/datasheet/2/302/PCA9698-1127696.pdf


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
Branch              Ra----Rofset        Rd----Opcode
Jump                Imm-----------------Rd----Opcode
Jump Offset         Imm-----------------Rd----Opcode
Jump Reg            Ra----              Rd----Opcode


## Signals
Ra      16
Rb      16
Rd      16
Raa     5
Rba     5
Rda     5
Rare    1
Rbre    1
Rdwr    1
        66

Busd    16
Busa    16
Busre   1
Buswr   1
        34

Aluoe   1
Alufunc 4
        5

        105