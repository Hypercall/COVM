using System;
using System.Collections.Generic;
using System.Text;

namespace COVM_IDE.Models.Compiler.Handler
{
    class NOTHandler : IInstructionHandler
    {
        public bool Compile(int instructionId, string instruction, ref List<byte> byteCode)
        {
            string[] commandsplit = instruction.Split(' ');
            commandsplit[1] = commandsplit[1].Replace(" ", "");
            var reg = Register.TranslateRegister(commandsplit[1]);
            if (!Register.IsRegister(commandsplit[1]))
                throw new Exception(instruction + ": First parameter is not a register");

            byteCode.Add((byte)Instructions.VM_Instruction.NOT);
            byteCode.Add(1);
            byteCode.Add((byte)reg);
            return true;
        }
    }
}
