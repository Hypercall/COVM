using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace COVM_IDE.Models.Compiler.Handler
{
    class NOPHandler : IInstructionHandler
    {
        public bool Compile(int instructionId, string instruction, ref List<byte> byteCode)
        {
            byteCode.Add((byte)Instructions.VM_Instruction.NOP);
            return true;
        }
    }
}
