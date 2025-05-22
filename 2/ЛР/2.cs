using System;

public interface IShape
{
	double CalculateArea();
	double CalculatePerimeter();
}

public class ShapeBase
{
	public string Name { get; protected set; }

	public ShapeBase(string name)
	{
		Name = name;
	}
}

public class Circle : ShapeBase, IShape
{
	public double Radius { get; private set; }

	public Circle(double radius) : base("Круг")
	{
		Radius = radius;
	}

	public double CalculateArea()
	{
		return Math.PI * Radius * Radius;
	}

	public double CalculatePerimeter()
	{
		return 2 * Math.PI * Radius;
	}
}

public class Square : ShapeBase, IShape
{
	public double SideLength { get; private set; }

	public Square(double sideLength) : base("Квадрат")
	{
		SideLength = sideLength;
	}

	public double CalculateArea()
	{
		return SideLength * SideLength;
	}

	public double CalculatePerimeter()
	{
		return 4 * SideLength;
	}
}

public class EquilateralTriangle : ShapeBase, IShape
{
	public double SideLength { get; private set; }

	public EquilateralTriangle(double sideLength) : base("Равносторонний треугольник")
	{
		SideLength = sideLength;
	}

	public double CalculateArea()
	{
		return (Math.Sqrt(3) / 4) * SideLength * SideLength;
	}

	public double CalculatePerimeter()
	{
		return 3 * SideLength;
	}
}

class Program
{
	static void Main(string[] args)
	{
		var circle = new Circle(5);
		var square = new Square(4);
		var triangle = new EquilateralTriangle(3);

		PrintShapeInfo(circle);
		PrintShapeInfo(square);
		PrintShapeInfo(triangle);
	}

	static void PrintShapeInfo(IShape shape)
	{
		if (shape is ShapeBase baseShape)
		{
			Console.WriteLine($"Фигура: {baseShape.Name}");
			Console.WriteLine($"Площадь: {shape.CalculateArea():F2}");
			Console.WriteLine($"Периметр: {shape.CalculatePerimeter():F2}\n");
		}
	}
}
