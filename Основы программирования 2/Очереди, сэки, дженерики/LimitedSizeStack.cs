using System;
using System.Collections.Generic;

namespace LimitedSizeStack;

public class LimitedSizeStack<T>
{
    LinkedList<T> list;
	int undoLimit = 0;
	
	public LimitedSizeStack(int undoLimit)
	{
		this.undoLimit = undoLimit;
        list = new LinkedList<T>();
    }

	public void Push(T item)
	{
		list.AddLast(item);
		if (Count > undoLimit) list.RemoveFirst(); 
	}

	public T Pop()
	{
		if(Count < 1) throw new NullReferenceException();

		T result = list.Last.Value;
		list.RemoveLast();
		return result;
	}

	public int Count => list.Count;
}