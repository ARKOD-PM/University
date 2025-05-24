using System;

class Program
{
	static void DFS(bool[,] matrix, bool[] visited, int currentVertex)
	{
		visited[currentVertex] = true;
		int size = matrix.GetLength(0);

		for (int neighbor = 0; neighbor < size; neighbor++)
		{
			if (matrix[currentVertex, neighbor] && !visited[neighbor])
			{
				DFS(matrix, visited, neighbor);
			}
		}
	}

	static int CountComponentsDFS(bool[,] matrix)
	{
		int size = matrix.GetLength(0);
		bool[] visited = new bool[size];
		int components = 0;

		for (int i = 0; i < size; i++)
		{
			if (!visited[i])
			{
				DFS(matrix, visited, i);
				++components;
			}
		}
		return components;
	}

	static int CountComponentsComponents(bool[,] matrix)
	{
		int size = matrix.GetLength(0);
		int[] components = new int[size];
		int currentComponent = 0;

		for (int i = 0; i < size; i++)
		{
			if (components[i] == 0)
			{
				components[i] = ++currentComponent;

				for (int j = i + 1; j < size; j++)
				{
					if (matrix[i, j] && components[j] == 0)
					{
						components[j] = currentComponent;
					}
				}
			}
		}

		for (int i = 0; i < size; i++)
		{
			for (int j = i + 1; j < size; j++)
			{
				if (matrix[i, j] && components[i] != components[j])
				{
					components[i] = components[j] = Math.Min(components[i], components[j]);
				}
			}
		}

		return components.Max();
	}

	static void Main()
	{
		Console.Write("Введите количество вершин: ");
		int n = GetValidInteger();
		bool[,] matrix = new bool[n, n];

		for (int i = 0; i < n; i++)
		{
			for (int j = i + 1; j < n; j++)
			{
				Console.Write($"Ребро ({i + 1}, {j + 1}) (1/0): ");
				bool hasEdge = GetValidBool();
				matrix[i, j] = matrix[j, i] = hasEdge;
			}
		}

		int resultDFS = CountComponentsDFS(matrix);
		int resultComponents = CountComponentsComponents(matrix);

		Console.WriteLine($"Компонент (DFS): {resultDFS}");
		Console.WriteLine($"Компонент (Метки): {resultComponents}");
	}
	
	static int GetValidInteger()
	{
		while(true)
		{
			string? input = Console.ReadLine();
			if (int.TryParse(input, out int value) && value > 0) return value;
			Console.WriteLine("Введите положительное целое число.");
		}
	}

	static bool GetValidBool()
	{
		while(true)
		{
			string? input = Console.ReadLine();
			if (input == "1") return true;
			if (input == "0") return false;
			Console.WriteLine("Введите 0 (нет ребра) или 1 (есть ребро).");
		}
	}
}
