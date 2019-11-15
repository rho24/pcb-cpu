
`include "registers.sv"

module top(
    input  [ 4:0] regAddrA,
    input  [ 4:0] regAddrB,
    input  [ 4:0] regAddrD,
    input         regReA,
    input         regReB,
    input         regWeD,
    output [15:0] busA,
    output [15:0] busB,
    input  [15:0] busD
);

registers registers(
    .regAddrA(regAddrA),
    .regAddrB(regAddrB),
    .regAddrD(regAddrD),
    .regReA(regReA),
    .regReB(regReB),
    .regWeD(regWeD),
    .busA(busA),
    .busB(busB),
    .busD(busD)
);

always @($global_clock) begin
    if (regAddrA == 0 && regReA) begin
        assert(busA == 0);
    end

    if (regAddrB == 0 && regReB) begin
        assert(busB == 0);
    end
    
    if (regReD == 5 && $fell(regWeD) && regReB == 5 && busD == 14) begin
        assert(regB == 14);
    end
end

endmodule
