using System;
using System.Collections.Generic;
using System.Linq;

namespace LimitedSizeStack;

public class ListModel<TItem>
{
	public List<TItem> Items { get; }
	public int UndoLimit;
	private LimitedSizeStack<(TItem, int)> stack;
        
	public ListModel(int undoLimit) : this(new List<TItem>(), undoLimit)
	{
	}

	public ListModel(List<TItem> items, int undoLimit)
	{
		Items = items;
		UndoLimit = undoLimit;
		stack = new LimitedSizeStack<(TItem, int)>(undoLimit);
	}

	public void AddItem(TItem item)
	{
		Items.Add(item);
		stack.Push((item, -1));
	}

	public void RemoveItem(int index)
	{
        stack.Push((Items[index], index));
        Items.RemoveAt(index);
    }

	public bool CanUndo()
	{
		return stack.Count > 0;
	}

	public void Undo()
	{
		var temp = stack.Pop();

		if (temp.Item2 != -1) Items.Insert(temp.Item2,temp.Item1);
        else Items.RemoveAt(Items.Count - 1);
    }
}