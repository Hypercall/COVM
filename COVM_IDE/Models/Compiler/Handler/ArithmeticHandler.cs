using System;
using System.Collections.Generic;
using System.Text;

namespace COVM_IDE.Models.Compiler.Handler
{
    class ArithmeticHandler : IInstructionHandler
    {
        public bool Compile(int instructionId, string instruction, ref List<byte> byteCode)
		{
			long val = 0;
			string[] commandsplit = instruction.Split(' ');
			string main_command = commandsplit[0];
			string[] register_comm = instruction.Replace(main_command, "").Split(',');
			byte[] bytes = null;

			byteCode.Add((byte)instructionId);
			if (register_comm.Length != 2)
				throw new Exception(instruction.ToString() + ": invalid parameter");
			register_comm[0] = register_comm[0].Replace(" ", "");
			register_comm[1] = register_comm[1].Replace(" ", "");
			if (!Register.IsRegister(register_comm[0]))
				throw new Exception(instruction.ToString() + ": First parameter is not a register");
			if (Register.IsRegister(register_comm[1]))
			{
				byteCode.Add(1);
				byteCode.Add((byte)Register.TranslateRegister(register_comm[0]));
				byteCode.Add((byte)Register.TranslateRegister(register_comm[1]));
				return true;
			}
			byteCode.Add(0);
			Int64.TryParse(register_comm[1], out val);
			var size = (byte)DataTypes.TranslateSize(val);
			byteCode.Add(size);
			byteCode.Add((byte)Register.TranslateRegister(register_comm[0]));
			if (size == 0)
				throw new Exception(instruction.ToString() + ": invalid value");
			if (size > 1)
			{
				switch (size)
				{
					case 2:
						bytes = BitConverter.GetBytes((short)val);
						break;
					case 4:
						bytes = BitConverter.GetBytes((int)val);
						break;
					case 8:
						bytes = BitConverter.GetBytes((long)val);
						break;
					default:
						throw new Exception(instruction.ToString() + ": invalid size");
				}
				foreach (var b in bytes)
					byteCode.Add(b);
			}
			else if (size == 1)
				byteCode.Add((byte)val);
			else if (size == 0)
				throw new Exception(instruction.ToString() + ": invalid size!");
			return true;
		}
    }
}
