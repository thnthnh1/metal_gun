using System;
using System.Collections.Generic;

public class ObjectPooling<T> where T : class, new()
{
	private Stack<T> m_objectStack;

	public int Count
	{
		get
		{
			return this.m_objectStack.Count;
		}
	}

	public ObjectPooling(int initialBufferSize = 8)
	{
		this.m_objectStack = new Stack<T>(initialBufferSize);
	}

	public T New()
	{
		T result = (T)((object)null);
		if (this.m_objectStack.Count > 0)
		{
			result = this.m_objectStack.Pop();
		}
		return result;
	}

	public void Store(T obj)
	{
		this.m_objectStack.Push(obj);
	}

	public void Clean()
	{
		while (this.Count > 8)
		{
			this.m_objectStack.Pop();
		}
		this.m_objectStack.TrimExcess();
	}
}
