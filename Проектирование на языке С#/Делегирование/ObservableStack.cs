using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delegates.Observers
{
	public class StackOperationsLogger
	{
		private readonly StringBuilder log = new StringBuilder();
		public void SubscribeOn<T>(ObservableStack<T> stack)
		{
            stack.Notify += (data) => log.Append(data?.ToString());
		}

		public string GetLog()
		{
			return log.ToString();
		}
	}

	public class ObservableStack<T>
	{
		public event Action<StackEventData<T>> Notify;

		List<T> data = new List<T>();

		public void Push(T obj)
		{
            data.Add(obj);
            Notify?.Invoke(new StackEventData<T> { IsPushed = true, Value = obj });
		}

		public T Pop()
		{
			if (data.Count == 0)
				throw new InvalidOperationException();
			var result = data[data.Count - 1];
			Notify?.Invoke(new StackEventData<T> { IsPushed = false, Value = result });
			return result;
		}
	}
}