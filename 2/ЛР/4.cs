using System;
using System.Collections.Generic;

public static class PolishCalculator
{
	public static int Evaluate(string input)
	{
		Stack<int> stack = new Stack<int>();
		string[] tokens = input.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

		foreach (string token in tokens)
		{
			if (int.TryParse(token, out int number))
			{
				stack.Push(number);
			}
			else if (IsOperator(token))
			{
				if (stack.Count < 2)
					throw new InvalidOperationException($"Недостаточно операндов для операции '{token}'");

				int second = stack.Pop();
				int first = stack.Pop();

				switch (token)
				{
					case "+":
						stack.Push(first + second);
						break;
					case "-":
						stack.Push(first - second);
						break;
					case "*":
						stack.Push(first * second);
						break;
					case "/":
						if (second == 0)
							throw new DivideByZeroException("Деление на ноль");
						stack.Push(first / second);
						break;
				}
			}
			else
			{
				throw new ArgumentException($"Неверный токен: '{token}'");
			}
		}

		if (stack.Count != 1)
			throw new InvalidOperationException("Некорректное выражение: в стеке остались лишние числа");

		return stack.Pop();
	}

	private static bool IsOperator(string token)
	{
		return token == "+" || token == "-" || token == "*" || token == "/";
	}
}

class Program
{
	static void Main()
	{
		Console.WriteLine("Введите выражение (например: '3 4 + 5 *'):");

		string input = Console.ReadLine();

		try
		{
			int result = PolishCalculator.Evaluate(input);
			Console.WriteLine($"\nРезультат: {result}");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"\nОшибка: {ex.Message}");
		}
	}
}
