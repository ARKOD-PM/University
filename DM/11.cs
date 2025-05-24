using System;
using System.Collections.Generic;
using System.Linq;

public class Arc
{
	public int Weight { get; set; }
	public int From { get; }
	public int To { get; }

	public Arc(int weight, int from, int to)
	{
		Weight = weight;
		From = from;
		To = to;
	}
}

class Program
{
	private static Dictionary<int, List<Arc>> graph = new Dictionary<int, List<Arc>>();

	static Tuple<int, int>? ValidateGraph(int[][] matrix)
	{
		int n = matrix.GetLength(0);
		List<int> sources = new List<int>();
		List<int> sinks = new List<int>();

		for (int i = 0; i < n; i++)
		{
			bool isSource = true;
			bool isSink = true;

			for (int j = 0; j < n; j++)
			{
				if (matrix[i][j] != 0) isSink = false;
				if (matrix[j][i] != 0) isSource = false;
			}

			if (isSource) sources.Add(i);
			if (isSink) sinks.Add(i);
		}

		if (sources.Count == 0 || sinks.Count == 0)
		{
			Console.WriteLine("Ошибка: граф не является транспортной сетью — нет источника или стока.");
			return null;
		}

		if (sources.Count > 1 || sinks.Count > 1)
		{
			Console.WriteLine("Ошибка: граф несвязный — несколько источников или стоков.");
			return null;
		}

		return new Tuple<int, int>(sources[0], sinks[0]);
	}

	static bool FindAugmentingPath(int source, int sink, Dictionary<int, (int parent, Arc arc)> parent)
	{
		Queue<int> queue = new Queue<int>();
		HashSet<int> visited = new HashSet<int>();
		queue.Enqueue(source);
		visited.Add(source);

		while (queue.Count > 0)
		{
			int u = queue.Dequeue();
			if (!graph.ContainsKey(u)) continue;

			foreach (var arc in graph[u])
			{
				int v = arc.To;
				if (!visited.Contains(v) && arc.Weight > 0)
				{
					parent[v] = (u, arc);
					visited.Add(v);
					queue.Enqueue(v);

					if (v == sink)
						return true;
				}
			}
		}

		return false;
	}

	static void BuildResidualNetwork(int[][] matrix)
	{
		int n = matrix.GetLength(0);
		graph.Clear();

		for (int i = 0; i < n; i++)
		{
			for (int j = 0; j < n; j++)
			{
				int capacity = matrix[i][j];
				if (capacity > 0)
				{
					if (!graph.ContainsKey(i)) graph[i] = new List<Arc>();
					graph[i].Add(new Arc(capacity, i, j));

					if (!graph.ContainsKey(j)) graph[j] = new List<Arc>();
					graph[j].Add(new Arc(0, j, i));
				}
			}
		}
	}

	static void Main()
	{
		Console.Write("Введите число вершин сети: ");
		int n = int.Parse(Console.ReadLine());

		int[][] matrix = new int[n][];
		Console.Write($"Введите строки матрицы через пробел (дуга - число, отсутствие - 0): ");
		for (int i = 0; i < n; i++)
		{
			Console.Write($"Строка {i + 1}: ");
			matrix[i] = Array.ConvertAll(Console.ReadLine().Split(), int.Parse);
		}

		var result = ValidateGraph(matrix);
		if (result == null)
			return;

		int source = result.Item1;
		int sink = result.Item2;

		BuildResidualNetwork(matrix);

		int maxFlow = 0;

		Dictionary<int, (int, Arc)> parent;

		while (FindAugmentingPath(source, sink, parent = new Dictionary<int, (int, Arc)>()))
		{
			int pathFlow = int.MaxValue;
			int v = sink;

			while (parent.ContainsKey(v))
			{
				var (u, arc) = parent[v];
				pathFlow = Math.Min(pathFlow, arc.Weight);
				v = u;
			}

			v = sink;
			while (parent.ContainsKey(v))
			{
				var (u, arc) = parent[v];
				arc.Weight -= pathFlow;

				Arc reverseArc = graph[arc.To].FirstOrDefault(e => e.To == arc.From && e.From == arc.To);
				if (reverseArc != null)
					reverseArc.Weight += pathFlow;

				v = u;
			}

			maxFlow += pathFlow;
		}

		Console.WriteLine($"Максимальный поток: {maxFlow}");
	}
}
