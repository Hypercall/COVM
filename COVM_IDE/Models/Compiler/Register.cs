using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COVM_IDE.Models.Compiler
{
    class Register
    {
		public static Dictionary<string, string> registers = new Dictionary<string, string>
		{
			{"ax", "16 bit AX register" },
			{"bx", "16 bit BX register" },
			{"cx", "16 bit CX register" },
			{"dx", "16 bit CX register" },
			{"eax", "32 bit EAX register"},
			{"ecx", "32 bit ECX register"},
			{"edx", "32 bit EDX register"},
			{"ebx", "32 bit EBX reigster"},
			{"esp", "32 bit ESP register"},
			{"ebp", "32 bit EBP register"},
			{"esi", "32 bit ESI register"},
			{"edi", "32 bit EDI register"},
			{"rax", "64 bit RAX register"},
			{"rcx", "64 bit RCX register"},
			{"rdx", "64 bít RDX register"},
			{"rbx", "64 bit RBX register"},
			{"rsp", "64 bit RSP register"},
			{"rbp", "64 bit RBP register"},
			{"rsi", "64 bit RSI register"},
			{"rdi", "64 bit RDI register"},
		};

		public enum VM_Register : byte
		{
			//16-bit 
			AX,
			BX,
			CX,
			DX,

			//32-bit
			EAX,
			ECX,
			EDX,
			EBX,
			ESP,
			EBP,
			ESI,
			EDI,

			//64-bit
			RAX,
			RCX,
			RDX,
			RBX,
			RSP,
			RBP,
			RSI,
			RDI,
		};

		public static Dictionary<string, string> GetRegisters()
		{
			return registers;
		}

		public static string GetRegisterDescription(string register)
		{
			for (int i = 0; i < registers.Count; i++)
			{
				if (register.Equals(registers.ElementAt(i).Key, StringComparison.OrdinalIgnoreCase))
					return registers.ElementAt(i).Value;
			}
			return "";
		}

		public static bool IsRegister(string register)
		{
			for (int i = 0; i < registers.Count; i++)
			{
				if (register.Equals(registers.ElementAt(i).Key, StringComparison.OrdinalIgnoreCase))
					return true;
			}
			return false;
		}

		public static VM_Register TranslateRegister(string register)
		{
			for (int i = 0; i < registers.Count; i++)
			{
				if (register.Equals(registers.ElementAt(i).Key, StringComparison.OrdinalIgnoreCase))
					return (VM_Register)i;
			}
			return 0;
		}
	}
}
