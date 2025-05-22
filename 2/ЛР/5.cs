using System;
using System.Collections.Generic;
using System.Linq;

partial class Program
{
	static void Main()
	{
		Console.WriteLine("Введите числа через пробел:");
		
		List<int> numbers = ReadNumbers();
		
		ShowUniqueElements(numbers);
		ShowMostFrequent(numbers);
	}

	static List<int> ReadNumbers()
	{
		while(true)
		{
			try
			{
				return Console.ReadLine()
					.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)
					.Select(int.Parse)
					.ToList();
			}
			catch (FormatException)
			{
				Console.WriteLine("Ошибка! Допустимы только целые числа. Повторите ввод:");
			}
		}
	}

	static void ShowUniqueElements(List<int> numbers)
	{
		var unique = new HashSet<int>(numbers);
		Console.WriteLine($"\nУникальные элементы: {string.Join(", ", unique.OrderBy(x => x))}");
	}

	static void ShowMostFrequent(List<int> numbers)
	{
		var frequency = numbers
			.GroupBy(n => n)
			.ToDictionary(g => g.Key, g => g.Count());

		if (!frequency.Any()) return;

		var maxCount = frequency.Values.Max();
		var topItems = frequency
			.Where(p => p.Value == maxCount)
			.Select(p => p.Key)
			.OrderBy(n => n);

		Console.WriteLine($"\nСамые частые элементы: {string.Join(", ", topItems)}");
		Console.WriteLine($"Количество повторений: {maxCount}");
	}
}
