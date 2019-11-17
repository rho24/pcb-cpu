module memory (
    input  [15:0] memAddr,
    input         memRe,
    input         memWe,
    input  [15:0] memWBus,
    output [15:0] memRBus
);
parameter initialMemHex = "";

reg [7:0] mem [15:0];

initial begin
    if (initialMemHex != "") begin
        $readmemh(initialMemHex, mem);
    end
end

always @(posedge memWe) begin
    mem[memAddr]         <= memWBus[ 7:0];
    mem[memAddr + 1]     <= memWBus[15:8];
end
assign memRBus = memRe ? {mem[memAddr + 1], mem[memAddr]} : 16'hz;

endmodule // memory