using NUnit.Framework;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;

namespace rocket_bot;

public class Channel<T> where T : class
{
    private List<T> list = new List<T>();
	
	public T this[int index]
	{
		get
		{
			lock (list)
			{
				if (index < list.Count) return list[index];
				else return null;
			}
		}
		set
		{
			lock(list)
			{
                if (list.Count > index)
				{
                    list.RemoveRange(index, list.Count - index);
					list.Add(value);
                }
                else if (list.Count == index)
                    list.Add(value);
            }
		}
	}

	public T LastItem()
	{
		lock (list)
		{
			if (list.Count > 0) return list.Last();
			else return null;
		}
	}

	public void AppendIfLastItemIsUnchanged(T item, T knownLastItem)
	{
		lock(list)
		{
			if(list.Count > 0 && knownLastItem == list.Last()) 
				list.Add(item);
			else if(list.Count == 0 && knownLastItem == null)
				list.Add(item);
		}
	}

	public int Count
	{
		get
		{
			lock(list)
			{
				return list.Count;
			}
		}
	}
}