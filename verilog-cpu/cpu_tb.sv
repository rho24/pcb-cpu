
`include "cpu.sv"

module top;

reg clk;
reg rst;

// cpu #("cpu_tb.mem") sut (.clk, .rst);
cpu #("../fsharp-cpu/bin/Debug/netcoreapp3.0/program.mem", 0) sut (.clk, .rst);


initial begin
    $dumpfile("out/test.vcd");
    $dumpvars(0, top);

    clk = 0;
    rst = 0;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    rst = 1;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    rst = 0;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
    #1 clk = !clk;
end

endmodule