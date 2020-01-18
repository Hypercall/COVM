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

int main()
{
	COVM VM(4096);
	VM.run(std::vector<std::uint8_t>
	{
		LSTART_BYTE(1), // Start function 
		MOV_EAX_STACK_BYTE(2), // Move current stack into eax
		ADD_EAX_BYTE(1), // add 1 to eax
		//RET_BYTE, // leave function
		ADD_EAX_BYTE(1), // repeat
		LEND_BYTE, // End of function
		NOP_BYTE, // placeholder, does nothing
		PUSH_EAX_BYTE(13), // Add 13 to stack
		CALL_BYTE(1) // Call the function
		
	});
	std::cout << "=============Register=============" << std::endl;
	VM.printRegister();
	std::cout << "==============Labels==============" << std::endl;
	VM.printLabels();
	std::cin.get();
	return 1;
}