using System;
using System.Collections.Generic;
using System.Linq;

namespace func.brainfuck
{
	public class BrainfuckBasicCommands
	{
		public static void RegisterTo(IVirtualMachine vm, Func<int> read, Action<char> write)
		{
			vm.RegisterCommand('.', b => { write((char)b.Memory[b.MemoryPointer]); });
			vm.RegisterCommand('+', b => { b.Memory[b.MemoryPointer]++; });
			vm.RegisterCommand('-', b => { b.Memory[b.MemoryPointer]--; });
            vm.RegisterCommand(',', b => { b.Memory[b.MemoryPointer] = (byte)read(); });
			vm.RegisterCommand('>', b => 
							   { if (b.MemoryPointer < b.Memory.Length - 1 ) b.MemoryPointer++; else b.MemoryPointer = 0; });
			vm.RegisterCommand('<', b => 
							   { if (b.MemoryPointer > 0) b.MemoryPointer--; else b.MemoryPointer = b.Memory.Length - 1; });

			RegisterLoop(vm, 'A', 'Z');
            RegisterLoop(vm, 'a', 'z');
            RegisterLoop(vm, '0', '9');
        }

        private static void RegisterLoop (IVirtualMachine vm,char start, char end)
        {
            for (char ch = start; ch <= end; ch++)
            {
                var c = ch;
                vm.RegisterCommand(c, b => { b.Memory[b.MemoryPointer] = (byte)c; });
            }
		}
	}
}