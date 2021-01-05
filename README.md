<h1 align="center">
  <br>
  <a href="https://hypercall.net"><img src="https://i.imgur.com/LR2Fr3W.png" alt="logo" width="200"></a>
  <br>
  COVM 
  <br>
</h1>

<h4 align="center">A lightweight, stack-based and assembly-like x86/x64 VM.</h4>

## Operators
1.  NOP - An operator that does nothing.
2.  ADD - Adds a number to a register
3.  SUB - Subtracts a number inside a register
4.  MUL - Multiplies a number inside a register with x
5.  DIV - Divides a number inside a register with x
6.  AND - Bitwise AND 
7.  XOR - Bitwise XOR 
8.  OR  - Bitwise OR
9.  NOT - Bitwise NOT
10. INC - Increases the number by one inside a register
11. DEC - Decreases the number by one inside a register
12. RET - Jumps to the end of the function
13. MOV - Moves data into a register 
14. PUSH - Pushes data into the stack
15. POP - Removes data from the stack
16. LSTART - Defines the start of a function
17. LEND - Defines the end of a function
18. CALL - Calls a function by index
19. JMP - Jumps to an instruction
20. CMP - Compares register to any given data (register or hard-coded value)
21. JT - Jumps if the above check was true
22. JNT - Jumps if the above check was false

more will be added later.

## Registers
Registers do not have a fixed size, but it is recommended to split them like the following:
- 16 bit-registers: AX, BX, CX, DX
- 32 bit-registers: EAX, ECX, EDX, EBX, ESP, EBP, ESI, EDI
- 64 bit-registers: RAX, RCX, RDX, RBX, RSP, RBP, RSI, RDI (R8, R9, ...)

## Flags
Currently the COVM does not make use of flags, here are ones planned for the future usage:
1. CF 
2. PF
3. AF
4. ZF
5. TF
6. IF
7. DF
8. OF
9. CTF

## Compiling

You need [Visual Studio](https://www.visualstudio.com) to compile COVM.

## License

*COVM* is licensed under GPLv2, which means you can freely distribute and/or modify the source of *COVM*.


