module alu (
    input         [ 3:0] aluFunc,
    input  signed [15:0] busA,
    input  signed [15:0] busB,
    input                aluEn,
    output signed [15:0] busD
);

reg [15:0] y;

always @(aluFunc or busA or busB)
case(aluFunc)
    0      : y = busA + busB; // ADD
    1      : y = busA - busB; // SUB
    2      : y = busA << 1; // SHIFT L
    3      : y = busA >> 1; // SHIFT R
    4      : y = busA & busB; // AND
    5      : y = busA | busB; // OR
    6      : y = busA ^ busB; // XOR
    7      : y = busA < busB ? -1 : busA == busB ? 0 : 1; // COMPARE
    default: y = 0;
endcase

assign busD = aluEn ? y : 16'hzzzz;

endmodule