using System;
using System.Collections.Generic;

class Program
{
	static void Dijkstra(int[,] matrix, int from, int n)
	{
		int[] distances = new int[n];
		Array.Fill(distances, int.MaxValue);

		bool[] visited = new bool[n];

		distances[from] = 0;

		for (int count = 0; count < n - 1; count++)
		{
			int minIndex = -1;
			int minDistance = int.MaxValue;

			for (int v = 0; v < n; v++)
			{
				if (!visited[v] && distances[v] <= minDistance)
				{
					minDistance = distances[v];
					minIndex = v;
				}
			}

			visited[minIndex] = true;

			for (int neighbor = 0; neighbor < n; neighbor++)
			{
				int edgeWeight = matrix[minIndex, neighbor];

				if (edgeWeight != int.MaxValue && !visited[neighbor])
				{
					int newDistance = distances[minIndex] + edgeWeight;

					if (newDistance < distances[neighbor])
					{
						distances[neighbor] = newDistance;
					}
				}
			}
		}

		Console.WriteLine("\nКратчайшие расстояния от вершины " + (from + 1) + ":");
		for (int i = 0; i < n; i++)
		{
			Console.WriteLine($"До вершины {i + 1}: {(distances[i] == int.MaxValue ? "∞" : distances[i].ToString())}");
		}
	}

	static int GetValidVertices()
	{
		while (true)
		{
			Console.Write("Введите количество вершин неориентированного графа: ");
			string? input = Console.ReadLine();
			if (int.TryParse(input, out int value) && value > 0) return value;
			Console.WriteLine("Граф должен содержать как минимум одну вершну.");
		}
	}

	static void Main()
	{
		int n = GetValidVertices();

		int[,] matrix = new int[n, n];
		int[] buffer = new int[n * n];
		Array.Fill(buffer, int.MaxValue);
		Buffer.BlockCopy(buffer, 0, matrix, 0, buffer.Length * sizeof(int));

		for (int i = 0; i < n; i++)
		{
			for (int j = i + 1; j < n; j++)
			{
				while (true)
				{
					Console.Write($"Введите вес ребра ({i + 1}, {j + 1}) или -, если ребро отсутствует: ");
					string? input = Console.ReadLine();
					if (int.TryParse(input, out int weight) && weight >= 0)
					{
						matrix[i, j] = matrix[j, i] = weight;
					}
					else if (input != "-")
					{
						Console.WriteLine("Неверный ввод");
						continue;
					}
					break;
				}
			}
		}

		int from;
		while (true)
		{
			Console.Write($"Введите стартовую вершину (1–{n}): ");
			string? input = Console.ReadLine();
			if (int.TryParse(input, out from) && from > 0 && from <= n) break;
			Console.WriteLine($"Введите число от 1 до {n}.");
		}

		Dijkstra(matrix, --from, n);
	}
}
