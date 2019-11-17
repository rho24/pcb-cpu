module registers(
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

reg [15:0] r [5:0];
initial begin
    r[0] <= 0;
end

always @(posedge regWeD) begin
    if (regAddrD != 0) begin
        r[regAddrD] <= busD;
    end
end

assign busA = (regReA) ? r[regAddrA] : 16'hzzzz;
assign busB = (regReB) ? r[regAddrB] : 16'hzzzz;

endmodule // registers