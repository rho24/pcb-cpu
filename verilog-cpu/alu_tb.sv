
`include "alu.sv"

module top;

reg         [ 3:0] aluFunc;
reg  signed [15:0] busA;
reg  signed [15:0] busB;
reg                aluEn;
wire signed [15:0] busD;

alu sut (.*);

initial begin
    $dumpfile("out/test.vcd");
    $dumpvars(0, top);

    aluFunc = 0; busA = 0; busB = 0; aluEn = 0;

    #10
    aluEn = 1;
    busA = 2;

    #10 busB = 3;
    #10 aluFunc = 1;
    #10 aluFunc = 2;
    #10 aluFunc = 3;
    #10 aluFunc = 4;
    #10 aluFunc = 5;
    #10 aluFunc = 6;
    #10 aluFunc = 7;
    #10 busA    = 3;
    #10 busB    = -7;

    #10

    aluEn = 0;
end
endmodule
