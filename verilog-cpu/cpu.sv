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
    output        memRe,
    output        memWe,
    output        regARe,
    output        regBRe,
    output        regDWe,
    output [15:0] memAddr,
    output [ 3:0] regAddrA,
    output [ 3:0] regAddrB,
    output [ 3:0] regAddrD
);
parameter initialMemHex = "";

control control (.*);

memory #(initialMemHex) memory (
    .memAddr,
    .memRe,
    .memWe,
    .memRBus(busD),
    .memWBus(busA)
);

endmodule