`include "alu.sv"
`include "control.sv"
`include "memory.sv"
`include "registers.sv"


module cpu (
    input         clk,
    input         rst,
    output [15:0] busA,
    output [15:0] busB,
    output [15:0] busD,
    output        clkHold,
    output [ 3:0] aluFunc,
    output        aluEn,
    output        memRe,
    output        memWe,
    output        regReA,
    output        regReB,
    output        regWeD,
    output [15:0] memAddr,
    output [ 4:0] regAddrA,
    output [ 4:0] regAddrB,
    output [ 4:0] regAddrD
);
parameter initialMemPath = "";
parameter isHex = 0;

control control (.*);
alu alu (.*);
registers registers (.*);

memory #(initialMemPath, isHex) memory (
    .memAddr,
    .memRe,
    .memWe,
    .memRBus(busD),
    .memWBus(busA)
);

endmodule