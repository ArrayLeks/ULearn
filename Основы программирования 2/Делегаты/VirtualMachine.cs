using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class VirtualMachine : IVirtualMachine
	{
		public string Instructions { get; }
		public int InstructionPointer { get; set; }
		public byte[] Memory { get; }
		public int MemoryPointer { get; set; }

		private Dictionary<char, Action<IVirtualMachine>> dict;

		public VirtualMachine(string program, int memorySize)
		{
			dict = new Dictionary<char, Action<IVirtualMachine>>();
			Memory = new byte[memorySize];
			Instructions = program;
			MemoryPointer = 0;
			InstructionPointer = 0;
		}

		public void RegisterCommand(char symbol, Action<IVirtualMachine> execute)
		{
			dict.Add(symbol, execute);
		}

		public void Run()
		{
			while (InstructionPointer < Instructions.Length)
			{
				if (!dict.ContainsKey(Instructions[InstructionPointer]))
				{
                    InstructionPointer++;
                    continue; 
				}
					
				dict[Instructions[InstructionPointer]].Invoke(this);
				InstructionPointer++;
			}
		}
	}
}