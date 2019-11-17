
`include "registers.sv"

module top;

reg  [ 4:0] regAddrA;
reg  [ 4:0] regAddrB;
reg  [ 4:0] regAddrD;
reg         regReA;
reg         regReB;
reg         regWeD;
wire [15:0] busA;
wire [15:0] busB;
reg  [15:0] busD;

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
