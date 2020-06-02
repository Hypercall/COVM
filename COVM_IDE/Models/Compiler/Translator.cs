using COVM_IDE.Models.Compiler.Handler;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace COVM_IDE.Models.Compiler
{
    class Translator
    {
        private CompilerContext context;

        public Translator()
        {
            context = new CompilerContext();
        }

        public CompilerContext GetContext()
        {
            return this.context;
        }

        public void Reset()
        {
            context = new CompilerContext();
        }

        public string[] ILHelper(string command)
        {
            List<string> Keywords = new List<string>();
            var instructionId = Instructions.TranslateInstruction(command);
            switch (instructionId)
            {
                case Instructions.VM_Instruction.CALL:
                    foreach (var label in context.Labels)
                        Keywords.Add(label.Key);
                    break;
                case Instructions.VM_Instruction.JMP:
                    foreach (var label in context.Labels)
                        Keywords.Add(label.Key);
                    break;
                default:
                    break;
            }
            return Keywords.ToArray();
        }

        public void Translate(string[] instructions)
        {
            context.Labels.Clear();
            ScanLabels(instructions);
            foreach (var instr in instructions)
            {
                var index_start = 0;
                for (index_start = 0; index_start < instr.Length; index_start++)
                {
                    if (instr[index_start] != ' ')
                        break;
                }
                var str = instr;
                if (index_start != 0)
                    str = instr.Substring(index_start, instr.Length - index_start);
                string[] commandsplit = str.Split(' ');
                string main_command = commandsplit[0].Replace(" ", "").Replace("\t", "");
                if (main_command.Length == 0)
                    continue;
                var instructionId = Instructions.TranslateInstruction(main_command);
                switch (instructionId)
                {
                    case Instructions.VM_Instruction.NOP:
                        new Handler.NOPHandler().Compile((int)instructionId, str, ref context.ByteCode);
                        break;
                    case Instructions.VM_Instruction.ADD:
                    case Instructions.VM_Instruction.SUB:
                    case Instructions.VM_Instruction.DIV:
                    case Instructions.VM_Instruction.MUL:
                    case Instructions.VM_Instruction.AND:
                    case Instructions.VM_Instruction.OR:
                    case Instructions.VM_Instruction.XOR:
                    case Instructions.VM_Instruction.MOV:
                        new Handler.ArithmeticHandler().Compile((int)instructionId, str, ref context.ByteCode);
                        break;
                    case Instructions.VM_Instruction.NOT:
                        new Handler.NOTHandler().Compile((int)instructionId, str, ref context.ByteCode);
                        break;
                    case Instructions.VM_Instruction.LSTART:
                        break;
                    case Instructions.VM_Instruction.LEND:
                        break;
                    case Instructions.VM_Instruction.CALL:
                        instructionId = 0;
                        foreach (var label in context.Labels)
                        {
                            if (label.Key.Equals(commandsplit[1], StringComparison.OrdinalIgnoreCase))
                            {
                                instructionId = (Instructions.VM_Instruction)label.Value;
                                break;
                            }
                        }
                        if (instructionId == 0)
                            throw new Exception("CALL : Label not found!");
                        new Handler.CALLHandler().Compile((int)instructionId, str, ref context.ByteCode);
                        break;
                    case Instructions.VM_Instruction.JMP:
                        instructionId = 0;
                        foreach (var label in context.Labels)
                        {
                            if (label.Key.Equals(commandsplit[1], StringComparison.OrdinalIgnoreCase))
                            {
                                instructionId = (Instructions.VM_Instruction)label.Value;
                                break;
                            }
                        }
                        if (instructionId == 0)
                            throw new Exception("JMP : Label not found!");
                        new Handler.JMPHandler().Compile((int)instructionId, str, ref context.ByteCode);
                        break;
                    case Instructions.VM_Instruction.RET:
                        new Handler.RETHandler().Compile((int)instructionId, str, ref context.ByteCode);
                        break;
                    case Instructions.VM_Instruction.INC:

                    
                    default:
                        throw new Exception("invalid operator : " + instr);
                }
                context.Instructions++;
            }
        }

        public void ScanLabels(string[] instructions)
        {
            foreach (var instr in instructions)
            {
                var index_start = 0;
                for (index_start = 0; index_start < instr.Length; index_start++)
                {
                    if (instr[index_start] != ' ')
                        break;
                }
                var str = instr;
                if (index_start != 0)
                    str = instr.Substring(index_start, instr.Length - index_start);
                string[] commandsplit = str.Split(' ');
                string main_command = commandsplit[0].Replace(" ", "").Replace("\t", "");
                if (main_command.Length == 0)
                    continue;
                var instructionId = Instructions.TranslateInstruction(main_command);
                switch (instructionId)
                {
                    case Instructions.VM_Instruction.LSTART:
                        bool isFound = false;
                        foreach (var label in context.Labels)
                        {
                            if (label.Key.Equals(commandsplit[1], StringComparison.OrdinalIgnoreCase))
                                isFound = true;
                        }
                        if (commandsplit[1].Length > 0 && !isFound)
                        {
                            context.Labels.Add(commandsplit[1], context.Labels.Count + 1);
                            new Handler.LSTARTHandler().Compile((int)context.Labels.Count, "", ref context.ByteCode);
                        }
                        break;
                    case Instructions.VM_Instruction.LEND:
                        new Handler.LENDHandler().Compile((int)instructionId, str, ref context.ByteCode);
                        break;
                    default:
                        break;
                }
            }
        }

    }
}
