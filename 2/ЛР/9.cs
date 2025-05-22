using System;

public class GenericArray<T>
{
	private T[] _array;
	private int _count;

	public GenericArray(int initialCapacity = 4)
	{
		_array = new T[initialCapacity];
		_count = 0;
	}

	public void Add(T item)
	{
		if (_count == _array.Length)
		{
			Array.Resize(ref _array, _array.Length * 2);
		}
		_array[_count++] = item;
	}

	public void RemoveAt(int index)
	{
		if (index < 0 || index >= _count)
		{
			throw new IndexOutOfRangeException("Неверный индекс");
		}

		for (int i = index; i < _count - 1; i++)
		{
			_array[i] = _array[i + 1];
		}
		_count--;
	}

	public T Get(int index)
	{
		if (index < 0 || index >= _count)
		{
			throw new IndexOutOfRangeException("Неверный индекс");
		}
		return _array[index];
	}

	public int Count => _count;
}

public class NumericOperations<T> where T : struct
{
	public T Value1 { get; set; }
	public T Value2 { get; set; }

	public NumericOperations(T val1, T val2)
	{
		Value1 = val1;
		Value2 = val2;
	}

	public T Add()
	{
		dynamic a = Value1;
		dynamic b = Value2;
		return a + b;
	}

	public T Subtract()
	{
		dynamic a = Value1;
		dynamic b = Value2;
		return a - b;
	}

	public T Multiply()
	{
		dynamic a = Value1;
		dynamic b = Value2;
		return a * b;
	}

	public T Divide()
	{
		dynamic a = Value1;
		dynamic b = Value2;
		if (b == 0) throw new DivideByZeroException();
		return a / b;
	}
}

class Program
{
	static void Main()
	{
		var intArray = new GenericArray<int>();
		intArray.Add(10);
		intArray.Add(20);
		Console.WriteLine(intArray.Get(1));
		
		var ops = new NumericOperations<double>(15.5, 3.2);
		Console.WriteLine(ops.Add());
		Console.WriteLine(ops.Divide());
	}
}

