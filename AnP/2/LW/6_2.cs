using System;

class IntClass
{
	public int First { get; set; }
	public int Second { get; set; }

	public IntClass(int first, int second)
	{
		First = first;
		Second = second;
	}

	public int Add(int x, int y) => x + y;
	public int Subtract(int x, int y) => x - y;
	public int Multiply(int x, int y) => x * y;
	
	public int Divide(int x, int y)
	{
		if (y == 0)
		{
			throw new DivideByZeroException("Ошибка: деление на ноль невозможно");
		}
		return x / y;
	}
}

class Program
{
	delegate int Operation(int a, int b);

	static void Main()
	{
		var obj = new IntClass(10, 5);

		Operation group1 = null;
		group1 += obj.Add;
		group1 += obj.Subtract;
		group1 += obj.Multiply;

		Operation group2 = null;
		group2 += obj.Multiply;
		group2 += (res, _) => obj.Subtract(res, obj.First);
		group2 += (res, _) => obj.Divide(res, obj.First);

		Console.WriteLine("Первая группа операций.");
		ExecuteOperations(group1, obj.First, obj.Second);

		Console.WriteLine("\nВторая группа операций.");
		ExecuteOperations(group2, obj.First, obj.Second);
	}

	static void ExecuteOperations(Operation operationGroup, int initialValue, int secondValue)
	{
		int result = initialValue;
		
		foreach (Operation op in operationGroup.GetInvocationList())
		{
			try
			{
				result = op(result, secondValue);
				Console.WriteLine($"Результат: {result}");
			}
			catch (DivideByZeroException ex)
			{
				Console.WriteLine($"Ошибка: {ex.Message}");
				return;
			}
		}
	}
}
