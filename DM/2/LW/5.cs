using System;
using System.Collections.Generic;

public class Arc
{
	public int From { get; set; }
	public int To { get; set; }
	public int Weight { get; set; }

	public Arc(int from, int to, int weight)
	{
		From = from;
		To = to;
		Weight = weight;
	}
}

class Program
{
	static void Main()
	{
		int n;
		while (true)
		{
			Console.Write("Введите количество вершин графа: ");
			if (int.TryParse(Console.ReadLine(), out n) && n > 0) break;
			Console.WriteLine("Граф должен содержать как минимум одну вершину.");
		}

		List<Arc> arcs = new List<Arc>();
		

		for (int i = 0; i < n; i++)
		{
			for (int j = 0; j < n; j++)
			{
				if (i == j) continue;

				while (true)
				{
					Console.Write($"Введите вес дуги ({i + 1}, {j + 1}) или -, если дуга отсутствует: ");
					string? input = Console.ReadLine();
					if (int.TryParse(input, out int weight))
					{
						arcs.Add(new Arc(i, j, weight));
					}
					else if (input != "-")
					{
						Console.WriteLine("Неверный ввод.");
						continue;
					}
					break;
				}
			}
		}

		int start;
		while (true)
		{
			Console.Write("\nВведите стартовую вершину (1 - {0}): ", n);
			if (int.TryParse(Console.ReadLine(), out start) && start > 0 && start <= n) break;
			Console.WriteLine($"Ошибка! Введите число от 1 до {n}");
		}
		start--;

		int[] distances = new int[n];
		int[] parents = new int[n];
		Array.Fill(distances, int.MaxValue);
		Array.Fill(parents, -1);
		distances[start] = 0;

		for (int i = 0; i < n - 1; i++)
		{
			bool updated = false;
			foreach (Arc arc in arcs)
			{
				if (distances[arc.From] != int.MaxValue &&
				distances[arc.To] > distances[arc.From] + arc.Weight)
				{
					distances[arc.To] = distances[arc.From] + arc.Weight;
					parents[arc.To] = arc.From;
					updated = true;
				}
			}
			if (!updated) break;
		}

		bool hasNegativeCycle = false;
		foreach (Arc arc in arcs)
		{
			if (distances[arc.From] != int.MaxValue &&
			distances[arc.To] > distances[arc.From] + arc.Weight)
			{
				hasNegativeCycle = true;
				break;
			}
		}

		if (hasNegativeCycle)
		{
			Console.WriteLine("\nОбнаружен отрицательный цикл.");
		}
		else
		{
			Console.WriteLine("\nРезультаты:");
			for (int i = 0; i < n; i++)
			{
				if (i == start) continue;

				if (distances[i] == int.MaxValue)
				{
					Console.WriteLine($"Пути до вершины {i + 1} нет.");
				}
				else
				{
					Console.WriteLine($"Вес пути до вершины {i + 1}: {distances[i]}");
					Console.Write("Путь: ");
					PrintPath(parents, start, i);
					Console.WriteLine();
				}
			}
		}
	}

	static void PrintPath(int[] parents, int start, int current)
	{
		if (current == start)
		{
			Console.Write($"{start + 1}");
		}
		else if (parents[current] == -1)
		{
			Console.Write("Путь отсутствует");
		}
		else
		{
			PrintPath(parents, start, parents[current]);
			Console.Write($" -> {current + 1}");
		}
	}
}
