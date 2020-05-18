using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COVM_IDE.Models.Compiler
{
    class Instructions
    {
		public static Dictionary<string, string> command_map = new Dictionary<string, string>()
		{
			{"nop", "An operator for nothing"},
			{"add", "adds values to register" },
			{"sub", "subtracts by register/value"},
			{"mul", "multiplies by value/register"},
			{"div", "divides by value/register"},
			{"and", "bit-wise and operator"},
			{"xor", "bit-wise xor operator" },
			{"or", "bit-wise or operator" },
			{"not", "bit-wise not operator" },
			{"inc", "increases reigster by one"},
			{"dec", "decreases register by one"},
			{"ret", "returns back to the position" },
			{"mov", "moves a specific value into a register"},
			{"push", "pushes a register or value into the stack" },
			{"pop", "pops the value"},
			{"lstart","Start of a label"},
			{"lend" , "End of label"},
			{"call", "calls a function by label or position"},
			{"jmp", "jmps to a specific position" },
			{"cmp", "compares register"},
			{"jt", "jump if true (label, position)"},
			{"jnt", "jump if not true (label, position)" }
		};

		public enum VM_Instruction : byte
		{
			NOP, /* (32) == 0 */
			ADD, /* */
			SUB,
			MUL,
			DIV,
			AND,
			XOR,
			OR,
			NOT,
			INC,
			DEC,
			RET,
			MOV,

			PUSH,
			POP,

			LSTART,
			LEND,

			CALL,
			JMP,

			CMP,

			JT,
			JNT

		};

		public static Dictionary<string, string> GetInstructions()
		{
			return command_map;
		}

		public static string GetInstructionDescription(string command)
		{
			for (int i = 0; i < command_map.Count; i++)
			{
				if (command.Equals(command_map.ElementAt(i).Key, StringComparison.OrdinalIgnoreCase))
					return command_map.ElementAt(i).Value;
			}
			return "";
		}

		public static bool IsInstruction(string command)
		{
			for (int i = 0; i < command_map.Count; i++)
			{
				if (command.Equals(command_map.ElementAt(i).Key, StringComparison.OrdinalIgnoreCase))
					return true;
			}
			return false;
		}

		public static VM_Instruction TranslateInstruction(string command)
		{
			for (int i = 0; i < command_map.Count; i++)
			{
				if (command.Equals(command_map.ElementAt(i).Key, StringComparison.OrdinalIgnoreCase))
					return (VM_Instruction)i;
			}
			return 0;
		}

		public static bool IsRegisterRequired(string command)
		{
			command = command.Replace(" ", "");
			switch (TranslateInstruction(command))
			{
				case VM_Instruction.ADD:
				case VM_Instruction.AND:
				case VM_Instruction.CMP:
				case VM_Instruction.DEC:
				case VM_Instruction.DIV:
				case VM_Instruction.INC:
				case VM_Instruction.JMP:
				case VM_Instruction.JNT:
				case VM_Instruction.JT:
				case VM_Instruction.MOV:
				case VM_Instruction.MUL:
				case VM_Instruction.NOT:
				case VM_Instruction.OR:
				case VM_Instruction.POP:
				case VM_Instruction.PUSH:
				case VM_Instruction.SUB:
				case VM_Instruction.XOR:
					return true;
				default:
					break;
			}
			return false;
		}

	}
}
