using System;

class Program
{
	static void Main()
	{
		int n = GetValidVertices();
		
		int[,] matrix = new int[n, n];
		int[] buffer = new int[n * n];
		Array.Fill(buffer, int.MaxValue);
		Buffer.BlockCopy(buffer, 0, matrix, 0, buffer.Length * sizeof(int));

		for (int i = 0; i < n; i++)
		{
			for (int j = 0; j < n; j++)
			{
				if (i == j)
				{
					matrix[i, j] = 0;
					continue;
				}

				while (true)
				{
					Console.Write($"Введите вес дуги ({i + 1}, {j + 1}) или -, если нет дуги: ");
					string? input = Console.ReadLine();
					if (int.TryParse(input, out int weight))
					{
						matrix[i, j] = weight;
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

		for (int k = 0; k < n; k++)
		{
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					if (matrix[i, k] != int.MaxValue && 
					matrix[k, j] != int.MaxValue &&
					matrix[i, j] > matrix[i, k] + matrix[k, j])
					{
						matrix[i, j] = matrix[i, k] + matrix[k, j];
					}
				}
			}
		}

		bool hasNegativeCycle = false;
		for (int i = 0; i < n; i++)
		{
			if (matrix[i, i] < 0)
			{
				hasNegativeCycle = true;
				break;
			}
		}

		if (hasNegativeCycle)
		{
			Console.WriteLine("\nОбнаружен цикл отрицательного веса.");
		}
		else
		{
			Console.WriteLine("\nМатрица кратчайших расстояний:");
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < n; j++)
				{
					if (matrix[i, j] == int.MaxValue) Console.Write("∞\t");
					else Console.Write($"{matrix[i, j]}\t");
				}
				Console.WriteLine();
			}
		}
	}
	
	static int GetValidVertices()
	{
		while (true)
		{
			Console.Write("Введите количество вершин графа: ");
			if (int.TryParse(Console.ReadLine(), out int value) && value > 0) return value;
			Console.WriteLine("Неверный ввод.");
		}
	}
		
}
