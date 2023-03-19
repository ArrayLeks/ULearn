using System.Collections.Generic;

namespace Clones;

public class CloneVersionSystem : ICloneVersionSystem
{
    Dictionary<int, Clone> clones;
    int currentIndex = 1;

    public CloneVersionSystem()
    {
        clones = new Dictionary<int, Clone>();
    }

    public string Execute(string query)
    {
        if (!clones.ContainsKey(1)) clones.Add(1, new Clone());
        string[] array = query.Split(' ');
        int.TryParse(array[1], out int index);
        
        if (array.Length > 2)
        {
            int.TryParse(array[2], out int programm);
            Learn(index, programm);
        }
        else if (array[0].StartsWith("rollback"))
            RollBack(index);
        else if (array[0].StartsWith("relearn"))
            ReLearn(index);
        else if (array[0].StartsWith("clone"))
            Clone(index);
        else if (array[0].StartsWith("check"))
            return Check(index);

        return null;
    }

    private void Learn(int index, int programm)
    {
        clones[index].Programs.Push(programm);
    }

    private void RollBack(int index)
    {
        if (clones.ContainsKey(index) && clones[index].Programs.Count > 0)
            clones[index].Versions.Push(clones[index].Programs.Pop());
    }

    private void ReLearn(int index)
    {
        clones[index].Programs.Push(clones[index].Versions.Pop());
    }

    private void Clone(int index)
    {
        currentIndex++;
        Clone clone = new Clone();
        clone.Programs = new MyStack(clones[index].Programs.Head,
            clones[index].Programs.Tail, clones[index].Programs.Count);
        clone.Versions = new MyStack(clones[index].Versions.Head,
            clones[index].Versions.Tail, clones[index].Versions.Count);
        clones.Add(currentIndex, clone);
    }

    private string Check(int index)
    {
        if (clones[index].Programs.Count == 0) return "basic";
        return clones[index].Programs.Check().ToString();
    }
}

struct Clone
{
    public MyStack Programs { get; set; }
    public MyStack Versions { get; set; }

    public Clone()
    {
        Programs = new MyStack();
        Versions = new MyStack();
    }
}

class StackNode
{
    public int Program { get; set; }
    public StackNode Prev { get; set; }
    public StackNode Next { get; set; }

    public StackNode(int program)
    {
        this.Program = program;
    }
}

class MyStack
{
    public StackNode Head { get; private set; }
    public StackNode Tail { get; private set; }
    public int Count { get; private set; }

    public MyStack()
    {
        Head = new StackNode(0);
        Tail = new StackNode(0);
        Count = 0;
    }

    public MyStack(StackNode head, StackNode tail, int count)
    {
        this.Head = head;
        this.Tail = tail;
        Count = count;
    }

    public void Push(int index)
    {
        StackNode stack = new StackNode(index);
        if (Count == 0)
            Head = Tail = stack;
        else if (Count == 1)
        {
            Tail = stack;
            stack.Prev = Head;
            Head.Next = Tail;
        }
        else
        {
            stack.Prev = Tail;
            Tail.Next = stack;
            Tail = stack;
        }
        Count++;
    }

    public int Pop()
    {
        if (Count == 0) throw new System.Exception("Stack is empty");

        int programm = Tail.Program;

        if (Count == 1) Head = Tail = null;
        else if (Count == 2) Tail = Head;
        else Tail = Tail.Prev;

        Count--;

        return programm;
    }

    public int Check()
    {
        return Tail.Program;
    }
}