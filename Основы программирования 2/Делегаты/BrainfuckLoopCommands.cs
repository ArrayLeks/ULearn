using System;
using System.Collections.Generic;

namespace func.brainfuck
{
	public class BrainfuckLoopCommands
	{
		public static void RegisterTo(IVirtualMachine vm)
		{
			var stack = new Stack<int>();
			var dict = new Dictionary<int, int>();

			Search(vm, stack, dict);

			vm.RegisterCommand('[', b => 
			{
				int index = vm.InstructionPointer;
				if (b.Memory[b.MemoryPointer] == 0)
					vm.InstructionPointer = dict[index];
			});

			vm.RegisterCommand(']', b => {
                int index = vm.InstructionPointer;
                if (b.Memory[b.MemoryPointer] != 0)
					vm.InstructionPointer = dict[index];
            });
		}

		private static void Search(IVirtualMachine vm, Stack<int> stack, Dictionary<int, int> dict)
		{
            for (int i = 0; i < vm.Instructions.Length; i++)
            {
                if (vm.Instructions[i] == '[') stack.Push(i);
                if (vm.Instructions[i] == ']')
                {
                    int j = stack.Pop();
                    dict.Add(i, j);
                    dict.Add(j, i);
                }
            }
        }
	}
}