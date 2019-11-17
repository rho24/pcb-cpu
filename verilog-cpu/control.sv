
`include "constants.sv"

module control (
    input             clk,
    input             rst,
    input      [15:0] busD,
    output reg        clkHold,
    output reg [ 3:0] aluFunc,
    output reg        aluEn,
    output reg        memRe,
    output reg        memWe,
    output reg        regReA,
    output reg        regReB,
    output reg        regWeD,
    output reg [15:0] memAddr,
    output reg [ 4:0] regAddrA,
    output reg [ 4:0] regAddrB,
    output reg [ 4:0] regAddrD
);

reg  [ 3:0] state;
reg  [ 3:0] nextState;

reg  [15:0] pc;
reg  [15:0] nextPc;

reg  [15:0] instructionUh;
reg  [15:0] instructionLh;
reg  [15:0] nextInstructionUh;
reg  [15:0] nextInstructionLh;

reg  [31:0] instruction;

assign instruction = {instructionUh, instructionLh};

initial begin
    state = 0;
    pc = 0;
    instructionUh = 0;
    instructionLh = 0;
end

always @(posedge clk) begin
    if (rst) begin
        state <= 7;
        pc <= 0;
        instructionUh <= 0;
        instructionLh <= 0;
    end else begin
        state <= nextState;
        pc <= nextPc;
        instructionUh <= nextInstructionUh;
        instructionLh <= nextInstructionLh;
    end
end

always @(state, busD, rst) begin
    clkHold = 0;
    aluFunc = 0;
    aluEn = 0;
    memRe = 0;
    memWe = 0;
    regReA = 0;
    regReB = 0;
    regWeD = 0;
    memAddr = 0;
    regAddrA = 0;
    regAddrB = 0;
    regAddrD = 0;

    nextState = state;
    nextPc = pc;
    nextInstructionUh = instructionUh;
    nextInstructionLh = instructionLh;

    case(state)
        0      : begin // FETCH1
            memAddr = pc;
            memRe   = 1;
            nextInstructionLh = busD;
            nextState = 1;
        end
        1      : begin // FETCH2
            memAddr = pc + 2;
            memRe   = 1;
            nextInstructionUh = busD;
            nextState = 2;
        end
        2      : begin // EXECUTE1
            nextState = 3;

            case(opcode)
                `OPCODE_ALU: begin
                    aluFunc  = instruction[14:12];
                    aluEn    = 1;
                    regReA   = 1;
                    regReB   = 1;
                    regAddrA = addrA;
                    regAddrB = addrB;
                    regAddrD = addrD;
                end
                `OPCODE_LOAD: begin
                    memAddr  = instruction[31:16];
                    memRe    = 1;
                    regAddrD = addrD;
                end
            endcase
        end
        3      : begin // EXECUTE2
            nextPc = pc + 4;
            nextState = 0;

            case(opcode)
                `OPCODE_ALU: begin
                    aluFunc  = instruction[14:12];
                    aluEn    = 1;
                    regReA   = 1;
                    regReB   = 1;
                    regWeD   = 1;
                    regAddrA = addrA;
                    regAddrB = addrB;
                    regAddrD = addrD;
                end
                `OPCODE_LOAD: begin
                    memAddr  = instruction[31:16];
                    memRe    = 1;
                    regAddrD = addrD;
                    regWeD   = 1;
                end
            endcase
        end
        6      : begin // HALT
            clkHold   = 1;
        end
        7      : begin // RST
            nextState = 0;
        end
    endcase
end

wire [6:0] opcode = instruction[ 6: 0];
wire [4:0] addrA  = instruction[19:15];
wire [4:0] addrB  = instruction[24:20];
wire [4:0] addrD  = instruction[11: 7];




endmodule