using System;
using System.Collections.Generic;
using System.Reflection.Emit;
using System.Text;

namespace COVM_IDE.Models.Compiler
{
    class CompilerContext
    {
        public IDictionary<string, int> Labels;
        public List<byte> ByteCode;
        public int Instructions;
        public CompilerContext()
        {
            Labels = new Dictionary<string, int>();
            ByteCode = new List<byte>();
            Instructions = 0;
        }
    }
}
