using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
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

	static List<Edge> BuildMST(int[,] matrix, int n)
	{
		bool[] visited = new bool[n];
		int[] key = new int[n];
		int[] parent = new int[n];
		Array.Fill(key, int.MaxValue);
		key[0] = 0;
		parent[0] = -1;

		List<Edge> mstEdges = new List<Edge>();

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
					parent[v] = u;
				}
			}
		}

		for (int v = 1; v < n; v++)
		{
			if (parent[v] != -1)
				mstEdges.Add(new Edge(matrix[v, parent[v]], new int[] { v, parent[v] }));
		}

		return mstEdges;
	}

	static int CountComponents(int[,] matrix, int n)
	{
		bool[] visited = new bool[n];
		int components = 0;

		for (int i = 0; i < n; i++)
		{
			if (!visited[i])
			{
				DFS(matrix, visited, i, n);
				components++;
			}
		}

		return components;
	}

	static void DFS(int[,] matrix, bool[] visited, int node, int n)
	{
		visited[node] = true;
		for (int i = 0; i < n; i++)
		{
			if (matrix[node, i] != int.MaxValue && !visited[i])
				DFS(matrix, visited, i, n);
		}
	}

	static bool IsBridge(int[,] matrix, int n, int u, int v, int initialComponents)
	{
		int originalWeight = matrix[u, v];
		matrix[u, v] = matrix[v, u] = int.MaxValue;

		int currentComponents = CountComponents(matrix, n);

		matrix[u, v] = matrix[v, u] = originalWeight;

		return currentComponents > initialComponents;
	}

	static void Main()
	{
		int n = GetCountVertices();

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
					if (int.TryParse(input, out int weight))
					{
						matrix[i, j] = matrix[j, i] = weight;
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

		int initialComponents = CountComponents(matrix, n);
		if (initialComponents > 1)
		{
			Console.WriteLine("Граф несвязный. Минимального остовного дерева не существует.");
			return;
		}

		List<Edge> mstEdges = BuildMST(matrix, n);
		List<Edge> bridges = new List<Edge>();
		foreach (var edge in mstEdges)
		{
			int u = edge.Vertices[0];
			int v = edge.Vertices[1];

			if (IsBridge(matrix, n, u, v, initialComponents))
				bridges.Add(edge);
		}

		if (bridges.Count == 0)
		{
			Console.WriteLine("В графе нет мостов.");
		}
		else
		{
			Console.WriteLine("Мосты в графе:");
			foreach (var edge in bridges)
			{
				int u = edge.Vertices[0];
				int v = edge.Vertices[1];
				Console.WriteLine($"Ребро ({u + 1}, {v + 1}), вес: {edge.Weight}");
			}
		}
	}

	static int GetCountVertices()
	{
		while(true)
		{
			Console.Write("Введите количество вершин неориентированного графа: ");
			string? input = Console.ReadLine();
			if (int.TryParse(input, out int value) && value > 0) return value;
			Console.WriteLine("Граф должен содержать как минимум одну вершину");
		}
	}
}
