using System;
using System.Collections.Generic;
using System.Text;

namespace COVM_IDE.Models.Compiler.Handler
{
    class LENDHandler : IInstructionHandler
    {
        public bool Compile(int instructionId, string instruction, ref List<byte> byteCode)
        {
            byteCode.Add((byte)Instructions.VM_Instruction.LEND);
            return true;
        }
    }
}
