module control (
    input         clk,
    input         rst,
    input  [15:0] busD,
    output reg        clkHold,
    output reg        memRe,
    output reg        memWe,
    output reg        regARe,
    output reg        regBRe,
    output reg        regDWe,
    output reg [15:0] memAddr,
    output reg [ 3:0] regAddrA,
    output reg [ 3:0] regAddrB,
    output reg [ 3:0] regAddrD
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
        state <= 0;
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
    memRe = 0;
    memWe = 0;
    regARe = 0;
    regBRe = 0;
    regDWe = 0;
    memAddr = 0;
    regAddrA = 0;
    regAddrB = 0;
    regAddrD = 0;

    nextPc = pc;
    nextInstructionUh = instructionUh;
    nextInstructionLh = instructionLh;
    if(!rst) begin
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
            2      : begin // DECODE
                nextState = 3;
            end
            3      : begin // EXECUTE1
                nextPc = pc + 4;
                nextState = 0;
            end
            7      : begin // HALT
                clkHold   = 1;
            end 
        endcase
    end
end

endmodule