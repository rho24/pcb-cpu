
`include "memory.sv"

module top;

reg  [15:0] memAddr;
reg         memRe;
reg         memWe;
reg  [15:0] memWBus;
wire [15:0] memRBus;

memory #("memory_tb.mem") memory (.*);

initial begin
    $dumpfile("out/test.vcd");
    $dumpvars(0, top);

    memAddr = 0; memRe = 0; memWe = 0; memWBus = 0;

    #10
    memAddr = 1;
    memRe   = 1;

    #10
    memAddr = 1;
    memRe   = 0;
    memWBus = 567;
    memWe   = 1;
    #1
    memWe   = 0;
    memRe   = 1;
    #10
    memRe   = 1;
end
endmodule
