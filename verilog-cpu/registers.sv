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

reg [15:0] r [31:0];
initial begin
    r[0] <= 0;
end

integer i;
always @(posedge regWeD) begin
    for (i=0;i<16;i=i+1) begin
        if (i == 0) r[i] <= 0;
        else if (i == regAddrD) r[i] <= busD;
        else r[i] <= r[i];
    end
end

always @(*) begin
    busA <= (regReA) ? r[regAddrA] : 16'hzzzz;
    busB <= (regReB) ? r[regAddrB] : 16'hzzzz;
end
endmodule // registers