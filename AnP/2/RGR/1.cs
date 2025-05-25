using System;
using System.IO;
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


		string[] inputFiles = Directory.GetFiles(testDir, "input*.txt");

		foreach (string inputFile in inputFiles)
		{
			try
			{
				string fileName = Path.GetFileName(inputFile);
				string numberPart = Regex.Match(fileName, @"\d+").Value;
				
				if (string.IsNullOrEmpty(numberPart))
				{
					Console.WriteLine($"Не удалось определить номер файла: {fileName}");
					continue;
				}

				string outputFileName = $"output{numberPart}.txt";
				string referenceFile = Path.Combine(testDir, outputFileName);
				
				string[] lines = File.ReadAllLines(inputFile);
				
				int s1 = int.Parse(lines[0].Split()[0]);
				int d1 = int.Parse(lines[0].Split()[1]);
				int s2 = int.Parse(lines[1].Split()[0]);
				int d2 = int.Parse(lines[1].Split()[1]);
				double R = double.Parse(lines[2]);

				double phi1 = ToRadians(s1);
				double lambda1 = ToRadians(d1);
				double phi2 = ToRadians(s2);
				double lambda2 = ToRadians(d2);

				double deltaPhi = phi2 - phi1;
				double deltaLambda = lambda2 - lambda1;

				double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) +
						   Math.Cos(phi1) * Math.Cos(phi2) *
						   Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);

				a = Math.Max(0, Math.Min(1, a)); // Защита от NaN
				double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(Math.Max(0, 1 - a)));
				double distance = R * c;

				string result = $"{distance:F3}";
				File.WriteAllText(outputFileName, result);
				
				if (File.Exists(referenceFile))
				{
					string referenceContent = File.ReadAllText(referenceFile).Trim();
					
					if (result.Trim() == referenceContent)
					{
						Console.WriteLine($"SUCCESS {fileName} -> {outputFileName}. Совпадает с эталоном.");
					}
					else
					{
						Console.WriteLine($"FAILED {fileName} -> {outputFileName}. Не совпадает с эталоном.");
						Console.WriteLine($"  Ожидаемое: {referenceContent}");
						Console.WriteLine($"  Получено:  {result}");
					}
				}
				else
				{
					Console.WriteLine($"WARNING {fileName} -> {outputFileName}. Обработан, но эталонный файл не найден.");
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"ERROR Ошибка при обработке файла: {ex.Message}");
			}
		}
	}

	static double ToRadians(double degrees)
	{
		return degrees * Math.PI / 180;
	}
}
