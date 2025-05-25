using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    struct Ball
    {
        public double X, Y, Z, R;
        public Ball(double x, double y, double z, double r)
        {
            X = x;
            Y = y;
            Z = z;
            R = r;
        }
    }

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
                var firstLine = lines[0].Split();
                double xn = double.Parse(firstLine[0]);
                double yn = double.Parse(firstLine[1]);
                double zn = double.Parse(firstLine[2]);
                double rn = double.Parse(firstLine[3]);

                Ball initial = new Ball(xn, yn, zn, rn);
                List<Ball> balls = new List<Ball> { initial };
                List<bool> hasIntersections = new List<bool> { false };
                int count = 1;
                int result = 0;

                int m = int.Parse(lines[1]);
                bool found = false;

                for (int i = 0; i < m && !found; i++)
                {
                    var parts = lines[i + 2].Split();
                    double xi = double.Parse(parts[0]);
                    double yi = double.Parse(parts[1]);
                    double zi = double.Parse(parts[2]);
                    double ri = double.Parse(parts[3]);

                    Ball current = new Ball(xi, yi, zi, ri);
                    bool newHas = false;

                    for (int j = 0; j < balls.Count; j++)
                    {
                        Ball other = balls[j];
                        double dx = other.X - xi;
                        double dy = other.Y - yi;
                        double dz = other.Z - zi;
                        double distSq = dx * dx + dy * dy + dz * dz;
                        double sumR = other.R + ri;
                        double sumR2 = sumR * sumR;

                        if (distSq < sumR2)
                        {
                            newHas = true;

                            if (!hasIntersections[j])
                            {
                                hasIntersections[j] = true;
                                count--;
                            }
                        }
                    }

                    balls.Add(current);

                    if (newHas)
                    {
                        hasIntersections.Add(true);
                        count--;
                    }
                    else
                    {
                        hasIntersections.Add(false);
                        count++;
                    }

                    if (count == 0)
                    {
                        result = i + 1;
                        found = true;
                    }
                }

                string outputResult = result == 0 ? "0" : result.ToString();
                File.WriteAllText(Path.Combine(testDir, outputFileName), outputResult);

                if (File.Exists(referenceFile))
                {
                    string expected = File.ReadAllText(referenceFile).Trim();
                    string actual = outputResult.Trim();

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
}
