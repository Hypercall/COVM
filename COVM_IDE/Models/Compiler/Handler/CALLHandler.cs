using System;
using System.Collections.Generic;
using System.Text;

namespace COVM_IDE.Models.Compiler.Handler
{
    class CALLHandler : IInstructionHandler
    {
        public bool Compile(int instructionId, string instruction, ref List<byte> byteCode)
        {
            byteCode.Add((byte)Instructions.VM_Instruction.CALL);
            byteCode.Add(0);
            byteCode.Add((byte)instructionId);
            return true;
        }
    }
}
