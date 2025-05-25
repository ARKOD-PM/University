using System;
using System.Collections.Generic;
using System.Linq;

public class Edge : IComparable<Edge>
{
	public int Weight { get; }
	public int[] Vertices { get; }

	public Edge(int weight, int[] vertices)
	{
		Weight = weight;
		Vertices = vertices;
	}

	public int CompareTo(Edge? other)
	{
		if (other is null)
			throw new ArgumentNullException(nameof(other));

		return Weight.CompareTo(other.Weight);
	}
}

public class UnionFind
{
	private int[] parent;

	public UnionFind(int n)
	{
		parent = Enumerable.Range(0, n).ToArray();
	}

	public int Find(int x)
	{
		return parent[x] == x ? x : Find(parent[x]);
	}

	public bool Union(int x, int y)
	{
		int rootX = Find(x);
		int rootY = Find(y);

		if (rootX == rootY) return false;

		parent[rootY] = rootX;
		return true;
	}
}

class Program
{
	static void Main()
	{
		List<Edge> edges = new List<Edge>();
		int n = GetCountVertices();

		for (int i = 0; i < n; i++)
		{
			for (int j = i + 1; j < n; j++)
			{
				while (true)
				{
					Console.Write($"Введите вес ребра ({i + 1}, {j + 1}) или -, если ребра нет: ");
					string? input = Console.ReadLine();
					if (int.TryParse(input, out int weight))
					{
						edges.Add(new Edge(weight, new int[] { i, j }));
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

		int? resultPrim = Prim(edges, n);
		if (resultPrim.HasValue)
			Console.WriteLine($"Результат (алгоритм Прима): {resultPrim.Value}");
		else
			Console.WriteLine("Алгоритм Прима. Граф несвязный. Минимальное остовное дерево не существует.");

		int? resultKruskal = Kruskal(edges, n);
		if (resultKruskal.HasValue)
			Console.WriteLine($"Результат (алгоритм Краскала): {resultKruskal.Value}");
		else
			Console.WriteLine("Алгоритм Краскала. Граф несвязный. Минимальное остовное дерево не существует.");
	}

	static int? Prim(List<Edge> edges, int n)
	{
		int[,] matrix = new int[n, n];
		int[] buffer = new int[n * n];
		Array.Fill(buffer, int.MaxValue);
		Buffer.BlockCopy(buffer, 0, matrix, 0, buffer.Length * sizeof(int));

		foreach (var edge in edges)
		{
			matrix[edge.Vertices[0], edge.Vertices[1]] = edge.Weight;
			matrix[edge.Vertices[1], edge.Vertices[0]] = edge.Weight;
		}

		bool[] visited = new bool[n];
		int[] key = new int[n];
		Array.Fill(key, int.MaxValue);
		key[0] = 0;

		for (int count = 0; count < n; count++)
		{
			int u = -1;
			for (int v = 0; v < n; v++)
			{
				if (!visited[v] && (u == -1 || key[v] < key[u]))
				{
					u = v;
				}
			}
			
			visited[u] = true;

			for (int v = 0; v < n; v++)
			{
				if (!visited[v] && matrix[u, v] < key[v])
				{
					key[v] = matrix[u, v];
				}
			}
		}
		
		if (key.Any(k => k == int.MaxValue)) return null;

		int sum = 0;
		foreach (int value in key)
		{
			sum += value;
		}
		return sum;
	}

	static int? Kruskal(List<Edge> edges, int n)
	{
		edges.Sort();
		UnionFind uf = new UnionFind(n);
		int result = 0;
		int edgeCount = 0;

		foreach (var edge in edges)
		{
			if (uf.Union(edge.Vertices[0], edge.Vertices[1]))
			{
				result += edge.Weight;
				++edgeCount;

				if (edgeCount == n - 1) break;
			}
		}

		if (edgeCount < n - 1) return null;
		return result;
	}

	static int GetCountVertices()
	{
		while (true)
		{
			Console.Write("Введите количество вершин неориентированного графа: ");
			string? input = Console.ReadLine();
			if (int.TryParse(input, out int value) && value > 1) return value;
			Console.WriteLine("Граф должен содержать как минимум две вершины.");
		}
	}
}
