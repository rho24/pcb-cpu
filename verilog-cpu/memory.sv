module memory (
    input  [15:0] memAddr,
    input         memRe,
    input         memWe,
    input  [15:0] memWBus,
    output [15:0] memRBus
);
parameter initialMemPath = "";
parameter isHex = 0;
int f, n_Temp;

reg [7:0] mem [0:65535];

initial begin
    if (initialMemPath != "") begin
        if (isHex) begin
            $readmemh(initialMemPath, mem);
        end else begin
            f = $fopen(initialMemPath, "rb");
            n_Temp = $fread(mem, f);
            $fclose(f);
        end
    end
end

always @(posedge memWe) begin
    mem[memAddr]         <= memWBus[ 7:0];
    mem[memAddr + 1]     <= memWBus[15:8];
end
assign memRBus = memRe ? {mem[memAddr], mem[memAddr + 1]} : 16'hz;

endmodule // memory