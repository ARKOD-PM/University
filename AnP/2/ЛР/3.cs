using System;
using System.Collections.Generic;

public class BracketValidator
{
	public static bool CheckBrackets(string input)
	{
		Stack<char> stack = new Stack<char>();
		Dictionary<char, char> bracketPairs = new Dictionary<char, char>
		{
			{ ')', '(' },
			{ ']', '[' },
			{ '}', '{' }
		};

		foreach (char c in input)
		{
			if (c == '(' || c == '[' || c == '{')
			{
				stack.Push(c);
			}
			else if (bracketPairs.ContainsKey(c))
			{
				if (stack.Count == 0)
					return false;

				char top = stack.Pop();
				if (top != bracketPairs[c])
					return false;
			}
		}

		return stack.Count == 0;
	}	
}

class Program
{
	static void Main()
	{
	Console.WriteLine("Введите строку для проверки (или нажмите Enter для выхода).\n");

	while(true)
	{
		Console.Write("Тест: ");
		string input = Console.ReadLine()!;

		if (string.IsNullOrEmpty(input)) break;

		bool result = BracketValidator.CheckBrackets(input);
		Console.WriteLine($"Результат: {(result ? "True" : "False")}\n");
		}
	}
}
