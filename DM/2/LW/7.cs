using System;
using System.Collections.Generic;

public class Pathfinder
{
	private int[,] waveMatrix;
	private bool[,] grid;
	private int width;
	private int height;
	
	private readonly int[] dx = { 0, 0, -1, 1 };
	private readonly int[] dy = { -1, 1, 0, 0 };

	public List<(int x, int y)> FindPath(bool[,] grid, (int x, int y) start, (int x, int y) end)
	{
		Initialize(grid, start, end);
		
		Queue<(int x, int y)> queue = new Queue<(int x, int y)>();
		queue.Enqueue(start);
		waveMatrix[start.x, start.y] = 0;

		while (queue.Count > 0)
		{
			var current = queue.Dequeue();
			
			for (int i = 0; i < 4; i++)
			{
				int nx = current.x + dx[i];
				int ny = current.y + dy[i];

				if (IsValidCell(nx, ny) && waveMatrix[nx, ny] == -1)
				{
					waveMatrix[nx, ny] = waveMatrix[current.x, current.y] + 1;
					queue.Enqueue((nx, ny));
					
					if (nx == end.x && ny == end.y) 
						return ReconstructPath(start, end);
				}
			}
		}
		
		return null;
	}

	private void Initialize(bool[,] grid, (int x, int y) start, (int x, int y) end)
	{
		this.grid = grid;
		width = grid.GetLength(0);
		height = grid.GetLength(1);
		
		waveMatrix = new int[width, height];
		
		for (int x = 0; x < width; x++)
			for (int y = 0; y < height; y++)
				waveMatrix[x, y] = grid[x, y] ? -1 : int.MinValue;

		if (!IsValidCell(start.x, start.y))
			throw new ArgumentException("Некорректная стартовая точка");
		
		if (!IsValidCell(end.x, end.y))
			throw new ArgumentException("Некорректная конечная точка");
	}

	private bool IsValidCell(int x, int y)
	{
		return x >= 0 && y >= 0 && x < width && y < height && grid[x, y];
	}

	private List<(int x, int y)> ReconstructPath((int x, int y) start, (int x, int y) end)
	{
		List<(int x, int y)> path = new List<(int x, int y)>();
		var current = end;
		
		while (current.x != start.x || current.y != start.y)
		{
			path.Add(current);
			
			for (int i = 0; i < 4; i++)
			{
				int px = current.x + dx[i];
				int py = current.y + dy[i];
				
				if (IsValidCell(px, py) && 
					waveMatrix[px, py] == waveMatrix[current.x, current.y] - 1)
				{
					current = (px, py);
					break;
				}
			}
		}
		
		path.Add(start);
		path.Reverse();
		return path;
	}
}

public class Program
{
	public static void Main()
	{
		bool[,] grid = InputGrid();
		var start = InputPoint("стартовой");
		var end = InputPoint("конечной");

		var pathfinder = new Pathfinder();
		
		try
		{
			var path = pathfinder.FindPath(grid, start, end);
			
			if (path != null)
			{
				Console.WriteLine("\nНайденный путь:");
				foreach (var (x, y) in path)
				{
					Console.WriteLine($"[{x}, {y}]");
				}
			}
			else
			{
				Console.WriteLine("Путь не существует!");
			}
		}
		catch (ArgumentException ex)
		{
			Console.WriteLine(ex.Message);
		}
	}

	private static bool[,] InputGrid()
	{
		Console.Write("Введите размеры сетки (строки столбцы): ");
		var size = Console.ReadLine().Split();
		int rows = int.Parse(size[0]);
		int cols = int.Parse(size[1]);

		bool[,] grid = new bool[rows, cols];
		
		Console.WriteLine("\nВведите сетку (0 - препятствие, 1 - проходимая клетка):");
		for (int i = 0; i < rows; i++)
		{
			string line;
			do
			{
				Console.Write($"Строка {i + 1}: ");
				line = Console.ReadLine().Trim();
			} while (line.Length != cols || !line.All(c => c == '0' || c == '1'));
			
			for (int j = 0; j < cols; j++)
			{
				grid[i, j] = line[j] == '1';
			}
		}
		return grid;
	}

	private static (int x, int y) InputPoint(string pointName)
	{
		Console.WriteLine($"\nВведите координаты {pointName} точки (строка столбец):");
		while (true)
		{
			var coords = Console.ReadLine().Split();
			if (coords.Length != 2)
			{
				Console.Write("Некорректный ввод. Попробуйте снова: ");
				continue;
			}
			
			if (int.TryParse(coords[0], out int x) && 
				int.TryParse(coords[1], out int y))
			{
				return (x, y);
			}
			Console.Write("Некорректные числа. Попробуйте снова: ");
		}
	}
}
