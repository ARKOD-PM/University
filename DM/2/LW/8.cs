using System;
using System.Collections.Generic;
using System.Linq;

public class LittleAlgorithm
{
	private static double bestCost = double.PositiveInfinity;
	private static List<Tuple<int, int>> bestPath = new List<Tuple<int, int>>();

	private static Tuple<double[][], double> ReduceMatrix(double[][] matrix)
	{
		int n = matrix.Length;
		double[][] newMatrix = matrix.Select(r => r.ToArray()).ToArray();
		double cost = 0;

		for (int i = 0; i < n; i++)
		{
			double min = newMatrix[i].Where(x => !double.IsPositiveInfinity(x)).DefaultIfEmpty().Min();
			if (min > 0 && !double.IsPositiveInfinity(min))
			{
				cost += min;
				for (int j = 0; j < n; j++)
				{
					if (!double.IsPositiveInfinity(newMatrix[i][j]))
						newMatrix[i][j] -= min;
				}
			}
		}

		for (int j = 0; j < n; j++)
		{
			double min = Enumerable.Range(0, n)
				.Select(i => newMatrix[i][j])
				.Where(x => !double.IsPositiveInfinity(x))
				.DefaultIfEmpty()
				.Min();
			
			if (min > 0 && !double.IsPositiveInfinity(min))
			{
				cost += min;
				for (int i = 0; i < n; i++)
				{
					if (!double.IsPositiveInfinity(newMatrix[i][j]))
						newMatrix[i][j] -= min;
				}
			}
		}

		return Tuple.Create(newMatrix, cost);
	}

	private static Tuple<int, int, double> FindMaxPenalty(double[][] matrix)
	{
		int n = matrix.Length;
		double maxPenalty = -1;
		int bestI = -1, bestJ = -1;

		for (int i = 0; i < n; i++)
		{
			for (int j = 0; j < n; j++)
			{
				if (matrix[i][j] != 0) continue;

				double minRow = double.PositiveInfinity;
				for (int k = 0; k < n; k++)
				{
					if (k != j && matrix[i][k] < minRow)
						minRow = matrix[i][k];
				}

				double minCol = double.PositiveInfinity;
				for (int k = 0; k < n; k++)
				{
					if (k != i && matrix[k][j] < minCol)
						minCol = matrix[k][j];
				}

				double penalty = (double.IsPositiveInfinity(minRow) ? 0 : minRow) +
								(double.IsPositiveInfinity(minCol) ? 0 : minCol);

				if (penalty > maxPenalty)
				{
					maxPenalty = penalty;
					bestI = i;
					bestJ = j;
				}
			}
		}

		return Tuple.Create(bestI, bestJ, maxPenalty);
	}

	private static bool IsComplete(List<Tuple<int, int>> path, int n)
	{
		if (path.Count != n) return false;

		var nextNode = new Dictionary<int, int>();
		var visited = new HashSet<int>();

		foreach (var (u, v) in path)
		{
			if (u == v || nextNode.ContainsKey(u)) return false;
			nextNode[u] = v;
			visited.Add(u);
		}

		if (visited.Count != n) return false;

		int current = path[0].Item1;
		for (int i = 0; i < n; i++)
		{
			if (!nextNode.ContainsKey(current)) return false;
			current = nextNode[current];
		}

		return current == path[0].Item1;
	}

	private static double[][] CopyMatrix(double[][] matrix)
	{
		return matrix.Select(r => r.ToArray()).ToArray();
	}

	public static void Main()
	{
		Console.Write("Введите число вершин графа: ");
		int n = int.Parse(Console.ReadLine());
		// думаю, что главное в коде - его работа, а исключения ... просто правильно вводить надо!

		double[][] matrix = new double[n][];
		Console.WriteLine("Введите матрицу весов (весы дуг через пробел, '-' - отсутствие дуги).");
		for (int i = 0; i < n; i++)
		{
			Console.Write($"Строка {i + 1}: ");
			matrix[i] = Console.ReadLine().Split()
				.Select(x => x.ToLower() == "-" ? double.PositiveInfinity : double.Parse(x))
				.ToArray();
		}

		var (initialMatrix, initialBound) = ReduceMatrix(matrix);
		var stack = new Stack<Tuple<double[][], List<Tuple<int, int>>, double>>();
		stack.Push(Tuple.Create(initialMatrix, new List<Tuple<int, int>>(), initialBound));

		while (stack.Count > 0)
		{
			var (currMatrix, path, cost) = stack.Pop();

			if (cost >= bestCost) continue;

			if (IsComplete(path, n))
			{
				if (cost < bestCost)
				{
					bestCost = cost;
					bestPath = new List<Tuple<int, int>>(path);
				}
				continue;
			}

			var (i, j, penalty) = FindMaxPenalty(currMatrix);
			if (i == -1 || j == -1) continue;

			var excludeMatrix = CopyMatrix(currMatrix);
			excludeMatrix[i][j] = double.PositiveInfinity;
			var (reducedExclude, excludeCost) = ReduceMatrix(excludeMatrix);
			stack.Push(Tuple.Create(reducedExclude, new List<Tuple<int, int>>(path), cost + excludeCost));

			var includeMatrix = CopyMatrix(currMatrix);
			for (int k = 0; k < n; k++)
			{
				includeMatrix[i][k] = double.PositiveInfinity;
				includeMatrix[k][j] = double.PositiveInfinity;
			}
			includeMatrix[j][i] = double.PositiveInfinity;
			var (reducedInclude, includeCost) = ReduceMatrix(includeMatrix);
			var newPath = new List<Tuple<int, int>>(path) { Tuple.Create(i, j) };
			stack.Push(Tuple.Create(reducedInclude, newPath, cost + includeCost));
		}

		Console.WriteLine("\nОптимальный путь:");
		foreach (var (u, v) in bestPath)
			Console.WriteLine($"{u + 1} → {v + 1}");
		Console.WriteLine($"Минимальная стоимость: {bestCost}");
	}
}
