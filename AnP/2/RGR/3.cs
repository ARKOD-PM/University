using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

class Program
{
    struct Position
    {
        public int X;
        public int Y;
        public Position(int x, int y)
        {
            X = x;
            Y = y;
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
                var coords = lines[1].Split();
                int X1 = int.Parse(coords[0]);
                int Y1 = int.Parse(coords[1]);
                int X2 = int.Parse(coords[2]);
                int Y2 = int.Parse(coords[3]);

                if (X1 == X2 && Y1 == Y2)
                {
                    File.WriteAllText(Path.Combine(testDir, outputFileName), "0");
                    Console.WriteLine($"SUCCESS {fileName} -> {outputFileName}. Совпадает с эталоном.");
                    continue;
                }

                int[,] distance = new int[N + 1, N + 1];
                for (int i = 0; i <= N; i++)
                {
                    for (int j = 0; j <= N; j++)
                    {
                        distance[i, j] = -1;
                    }
                }

                Queue<Position> queue = new Queue<Position>();
                distance[X1, Y1] = 0;
                queue.Enqueue(new Position(X1, Y1));

                bool found = false;

                while (queue.Count > 0)
                {
                    Position current = queue.Dequeue();
                    int x = current.X;
                    int y = current.Y;

                    if (x == X2 && y == Y2)
                    {
                        found = true;
                        break;
                    }

                    bool isBlack = (x + y) % 2 == 0;

                    List<Position> nextPositions = new List<Position>();

                    if (isBlack)
                    {
                        int[] dx = { 2, 2, -2, -2, 1, 1, -1, -1 };
                        int[] dy = { 1, -1, 1, -1, 2, -2, 2, -2 };
                        for (int i = 0; i < 8; i++)
                        {
                            int newX = x + dx[i];
                            int newY = y + dy[i];
                            if (newX >= 1 && newX <= N && newY >= 1 && newY <= N)
                            {
                                if (distance[newX, newY] == -1)
                                {
                                    nextPositions.Add(new Position(newX, newY));
                                }
                            }
                        }
                    }
                    else
                    {
                        int[] dx = { 0, 0, 1, -1 };
                        int[] dy = { 1, -1, 0, 0 };
                        for (int i = 0; i < 4; i++)
                        {
                            int newX = x + dx[i];
                            int newY = y + dy[i];
                            if (newX >= 1 && newX <= N && newY >= 1 && newY <= N)
                            {
                                if (distance[newX, newY] == -1)
                                {
                                    nextPositions.Add(new Position(newX, newY));
                                }
                            }
                        }
                    }

                    foreach (var next in nextPositions)
                    {
                        distance[next.X, next.Y] = distance[x, y] + 1;
                        queue.Enqueue(next);
                    }
                }

                string result = found ? $"{distance[X2, Y2]}" : "NO";

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
}
