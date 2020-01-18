#include "COVM.hpp"

/* Sample instructions */
#define NOP_BYTE COVM_Instruction::NOP
#define ADD_EAX_BYTE(x) COVM_Instruction::ADD, 0, 1, 2, x
#define MOV_EAX_BYTE(x) COVM_Instruction::MOV, 0, 1, 2, x
#define MOV_EAX_STACK_BYTE(x) COVM_Instruction::MOV, 2, 1, x
#define SUB_EAX_BYTE(x) COVM_Instruction::SUB, 0, 1, 2, x
#define XOR_EAX_BYTE(x) COVM_Instruction::SUB, 0, 1, 2, x
#define PUSH_EAX_BYTE(x) COVM_Instruction::PUSH, 0, 1, x
#define POP_EAX_BYTE(x) COVM_Instruction::POP, 0, 1, x
#define LSTART_BYTE(x) COVM_Instruction::LSTART, 0, x
#define LEND_BYTE COVM_Instruction::LEND
#define CALL_BYTE(x) COVM_Instruction::CALL, 0, x
#define RET_BYTE COVM_Instruction::RET
#define CMP_BYTE_EAX(x) COVM_Instruction::CMP, 1, 0, 1, 2, x
#define JNT_BYTE_EAX(x) COVM_Instruction::JNT, 2, x 
#define JMP_BYTE(x) COVM_Instruction::JMP, 2, x 
#define JT_BYTE_EAX(x) COVM_Instruction::JT, 2, x 

int main()
{
	COVM VM(4096);
	VM.run(std::vector<std::uint8_t>
	{
		LSTART_BYTE(1), // Start of function  1
		ADD_EAX_BYTE(1), // Add one to eax
		LEND_BYTE, // End of function 1

		LSTART_BYTE(2), // Start of function 2
		ADD_EAX_BYTE(1), // Move current stack into eax
		CMP_BYTE_EAX(100), // If eax reached 100 
		JT_BYTE_EAX(1), // Jump if true (to label 1)
		JMP_BYTE(2), // Jump always if not true (to label 2)
		LEND_BYTE, // End of function 2

		CALL_BYTE(2) // First instruction, call function 2
	});


	std::cout << "=============Register=============" << std::endl;
	VM.printRegister();
	std::cout << "==============Labels==============" << std::endl;
	VM.printLabels();
	std::cout << "==============Flags==============" << std::endl;
	VM.printFlags();
	std::cin.get();
	return 1;
}