using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class Program
{
	static void Main()
	{
		string directory = Directory.GetCurrentDirectory();
		string testDir = Path.Combine(directory, "tests");

		if (!Directory.Exists(testDir))
		{
			Console.WriteLine("Папка 'tests' не найдена.");
			return;
		}

		string[] inputFiles = Directory.GetFiles(testDir, "input_s1_*.txt");

		foreach (string inputFile in inputFiles)
		{
			try
			{
				string fileName = Path.GetFileName(inputFile);
				string numberPart = Regex.Match(fileName, @"(?<=input_s1_)\d+").Value;

				if (string.IsNullOrEmpty(numberPart))
				{
					Console.WriteLine($"Не удалось определить номер файла: {fileName}");
					continue;
				}

				string outputFileName = $"output_s1_{numberPart}.txt";
				string referenceFile = Path.Combine(testDir, outputFileName);

				string[] lines = File.ReadAllLines(inputFile);
				int N = int.Parse(lines[0]);

				List<(double X, double Y)> points = new List<(double X, double Y)>();
				for (int i = 1; i <= N; i++)
				{
					var coords = lines[i].Split();
					double x = double.Parse(coords[0]);
					double y = double.Parse(coords[1]);
					points.Add((x, y));
				}

				var convexHull = ComputeConvexHull(points);

				double area = CalculatePolygonArea(convexHull);

				string result = $"{area:F3}";
				File.WriteAllText(Path.Combine(testDir, outputFileName), result);

				if (File.Exists(referenceFile))
				{
					string expected = File.ReadAllText(referenceFile).Trim();
					string actual = result.Trim();

					if (expected == actual)
					{
						Console.WriteLine($"SUCCESS {fileName} -> {outputFileName}. Совпадает с эталоном.");
					}
					else
					{
						Console.WriteLine($"FAILED {fileName} -> {outputFileName}. Не совпадает с эталоном.");
						Console.WriteLine($"  Ожидаемое: {expected}");
						Console.WriteLine($"  Получено:  {actual}");
					}
				}
				else
				{
					Console.WriteLine($"WARNING {fileName} -> {outputFileName}. Обработан, но эталонный файл не найден.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ERROR Ошибка при обработке файла {Path.GetFileName(inputFile)}: {ex.Message}");
			}
		}
	}

	static List<(double X, double Y)> ComputeConvexHull(List<(double X, double Y)> points)
	{
		if (points.Count == 0) return new List<(double X, double Y)>();
		if (points.Count == 1)
		{
			return new List<(double X, double Y)> { points[0] };
		}

		var hull = new List<(double X, double Y)>();
		int n = points.Count;

		int leftmost = 0;
		for (int i = 1; i < n; i++)
		{
			if (points[i].X < points[leftmost].X || 
			   (points[i].X == points[leftmost].X && points[i].Y < points[leftmost].Y))
			{
				leftmost = i;
			}
		}

		int current = leftmost;
		do
		{
			hull.Add(points[current]);
			int next = (current + 1) % n;

			for (int i = 0; i < n; i++)
			{
				int orientation = Orientation(points[current], points[i], points[next]);
				if (orientation == 1 || 
				   (orientation == 0 && Distance(points[current], points[i]) > Distance(points[current], points[next])))
				{
					next = i;
				}
			}

			current = next;
		} while (current != leftmost && hull.Count <= n); // Защита от бесконечного цикла

		return hull;
	}

	static int Orientation((double X, double Y) p, (double X, double Y) q, (double X, double Y) r)
	{
		double val = (q.Y - p.Y) * (r.X - q.X) - 
					 (q.X - p.X) * (r.Y - q.Y);
		if (val == 0) return 0;
		return (val > 0) ? 1 : 2;
	}

	static double Distance((double X, double Y) p, (double X, double Y) q)
	{
		return Math.Sqrt((p.X - q.X) * (p.X - q.X) + (p.Y - q.Y) * (p.Y - q.Y));
	}

	static double CalculatePolygonArea(List<(double X, double Y)> polygon)
	{
		int n = polygon.Count;
		double area = 0;

		for (int i = 0; i < n; i++)
		{
			int j = (i + 1) % n;
			area += (polygon[i].X * polygon[j].Y - polygon[j].X * polygon[i].Y);
		}

		return Math.Abs(area) / 2.0;
	}
}
