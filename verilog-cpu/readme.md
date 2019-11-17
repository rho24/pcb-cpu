# commands
iverilog -g2012 -o out/registers_tb.vvp registers_tb.sv; vvp out/registers_tb.vvp
iverilog -g2012 -o out/memory_tb.vvp memory_tb.sv; vvp out/memory_tb.vvp
iverilog -g2012 -o out/alu_tb.vvp alu_tb.sv; vvp out/alu_tb.vvp
iverilog -g2012 -o out/cpu_tb.vvp cpu_tb.sv; vvp out/cpu_tb.vvp