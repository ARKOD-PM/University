using System;

public class Point
{
	public int X { get; private set; }
	public int Y { get; private set; }

	public Point(int x, int y)
	{
		X = x;
		Y = y;
	}

	public void Move(int deltaX, int deltaY)
	{
		X += deltaX;
		Y += deltaY;
	}

	public override string ToString() => $"({X}, {Y})";
}

public class RectangleArea
{
	public int MinX { get; }
	public int MaxX { get; }
	public int MinY { get; }
	public int MaxY { get; }

	public RectangleArea(int x1, int x2, int y1, int y2)
	{
		if (x1 >= x2 || y1 >= y2)
			throw new ArgumentException("Некорректные границы области");

		MinX = x1;
		MaxX = x2;
		MinY = y1;
		MaxY = y2;
	}

	public bool IsInside(Point point)
	{
		return point.X >= MinX && point.X <= MaxX && 
		point.Y >= MinY && point.Y <= MaxY;
	}
}

public class AreaMonitor
{
	public delegate void ExitEventHandler(string message);
	public event ExitEventHandler OnExit;

	private readonly RectangleArea _area;

	public AreaMonitor(RectangleArea area)
	{
		_area = area;
	}

	public void CheckPoint(Point point)
	{
		if (!_area.IsInside(point))
		{
			string direction = GetExitDirection(point);
			OnExit?.Invoke($"Точка {point} вышла за границы! Направление: {direction}");
		}
	}

	private string GetExitDirection(Point point)
	{
		string direction = "";
		if (point.X < _area.MinX) direction += "влево ";
		if (point.X > _area.MaxX) direction += "вправо ";
		if (point.Y < _area.MinY) direction += "вниз ";
		if (point.Y > _area.MaxY) direction += "вверх ";
		return direction.Trim();
	}
}

class Program
{
	static void Main()
	{
		Random rand = new Random();

		var area = new RectangleArea(0, 100, 0, 30);
		var monitor = new AreaMonitor(area);

		var point = new Point(
		rand.Next(area.MinX + 1, area.MaxX - 1),
		rand.Next(area.MinY + 1, area.MaxY - 1)
		);

		monitor.OnExit += message => Console.WriteLine($"СОБЫТИЕ: {message}");

		Console.WriteLine($"Начальная позиция: {point}");
		Console.WriteLine($"Границы области: X[{area.MinX}-{area.MaxX}], Y[{area.MinY}-{area.MaxY}]\n");

		for (int i = 0; i < 15; i++)
		{
			int dx = rand.Next(-15, 16);
			int dy = rand.Next(-15, 16);

			point.Move(dx, dy);
			Console.WriteLine($"Шаг {i+1}: Перемещение на ({dx}, {dy})");
			Console.WriteLine($"Новая позиция: {point}");

			monitor.CheckPoint(point);
			Console.WriteLine(new string('-', 50));
		}
	}
}
