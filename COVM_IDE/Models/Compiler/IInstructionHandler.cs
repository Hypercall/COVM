using System;
using System.Collections.Generic;
using System.Text;

namespace COVM_IDE.Models.Compiler
{
    interface IInstructionHandler
    {
        bool Compile(int instructionId, string instruction, ref List<byte> byteCode);
    }
}
