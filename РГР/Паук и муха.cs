using System;
using System.IO;

class Program
{
    static void Main()
    {
        string[] inputFiles = Directory.GetFiles("test/", "input*.txt");
        using (StreamWriter failedTestsWriter = new StreamWriter("failed_tests.txt"))
        {
            foreach (string inputFile in inputFiles)
            {
                string outputFile = inputFile.Replace("input", "output");

                if (File.Exists(outputFile))
                {
                    try
                    {
                        // Чтение данных из файла input.txt
                        string[] inputLines = File.ReadAllLines(inputFile);
                        double[] dim = Array.ConvertAll(inputLines[0].Split(), double.Parse);
                        double[] A = Array.ConvertAll(inputLines[1].Split(), double.Parse);
                        double[] B = Array.ConvertAll(inputLines[2].Split(), double.Parse);

                        // Вычисление минимального расстояния
                        double calculatedDistance = CalculateMinimumDistance(dim, A, B);

                        // Чтение ожидаемого результата из файла output.txt
                        double expectedDistance = double.Parse(File.ReadAllText(outputFile));

                        // Сравнение вычисленного и ожидаемого результата
                        if (Math.Abs(calculatedDistance - expectedDistance) < 0.001)
                        {
                            Console.WriteLine($"Тест {inputFile} пройден. Вычисленное расстояние совпадает с ожидаемым.");
                        }
                        else
                        {
                            Console.WriteLine($"Тест {inputFile} не пройден. Вычисленное расстояние: {calculatedDistance.ToString("F3")}, ожидаемое: {expectedDistance.ToString("F3")}");
                            failedTestsWriter.WriteLine($"Тест {inputFile} не пройден.");
                            failedTestsWriter.WriteLine($"Размеры комнаты: длина={dim[0]}, ширина={dim[1]}, высота={dim[2]}");
                            failedTestsWriter.WriteLine($"Координаты паука: x1={A[0]}, y1={A[1]}, z1={A[2]}");
                            failedTestsWriter.WriteLine($"Координаты мухи: x2={B[0]}, y2={B[1]}, z2={B[2]}");
                            failedTestsWriter.WriteLine($"Вычисленное расстояние: {calculatedDistance.ToString("F3")}, ожидаемое: {expectedDistance.ToString("F3")}");
                            failedTestsWriter.WriteLine();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Ошибка при обработке {inputFile}: {ex.Message}");
                        failedTestsWriter.WriteLine($"Ошибка при обработке {inputFile}: {ex.Message}");
                    }
                }
                else
                {
                    Console.WriteLine($"Для {inputFile} не найден файл {outputFile}");
                    failedTestsWriter.WriteLine($"Для {inputFile} не найден файл {outputFile}");
                }
            }
        }
    }

    static double CalculateMinimumDistance(double[] dim, double[] A, double[] B)
    {
        for (int i = 0; i < 3; i++)
        {
            int j = (i + 1) % 3,
                k = (j + 1) % 3;

            // A и B на одной грани.
            if (A[i] == 0 && B[i] == 0 || A[i] == dim[i] && B[i] == dim[i])
            {
                return Math.Sqrt(Math.Pow(B[j] - A[j], 2) + Math.Pow(B[k] - A[k], 2));
            }
        }

        double minDist = double.MaxValue,
               dist;
        double[] unB, distA, distB;
        int a, b, d;
        bool back = false;

        for (int i = 0; i < 3; i++)
        {
            int j = (i + 1) % 3,
                k = (j + 1) % 3;
            // A и B на смежных гранях.
            for (int _ = 0; _ < 2; _++)
            {
                (i, j) = (j, i);
                if ((A[i] == 0 || A[i] == dim[i]) && (B[j] == 0 || B[j] == dim[j]))
                {
                    d = (dim[k] < A[k] + B[k]) ? 1 : 0;
                    a = B[j] != 0 ? 1 : 0;
                    b = A[i] != 0 ? 1 : 0;

                    unB = new double[dim.Length];
                    distA = new double[dim.Length];
                    distB = new double[dim.Length];

                    distA[j] = a * dim[j] + Math.Pow(-1, a + 2) * A[j];
                    distB[k] = d * dim[k] + Math.Pow(-1, d + 2) * B[k];
                    distA[k] = d * dim[k] + Math.Pow(-1, d + 2) * A[k];
                    distB[i] = b * dim[i] + Math.Pow(-1, b + 2) * B[i];

                    unB[j] = distA[j] + distB[k];
                    unB[k] = distA[k] + distB[i];

                    if (unB[k] / unB[j] * distA[j] < distA[k])
                    {
                        minDist = Math.Min(minDist, Math.Sqrt(Math.Pow(distA[j], 2) + Math.Pow(distA[k], 2)) + Math.Sqrt(Math.Pow(distB[k], 2) + Math.Pow(distB[i], 2)));
                    }
                    else
                    {
                        minDist = Math.Min(minDist, Math.Sqrt(Math.Pow(unB[j], 2) + Math.Pow(unB[k], 2)));
                    }

                    return Math.Min(minDist, Math.Sqrt(Math.Pow(distA[j] + distB[i], 2) + Math.Pow(B[k] - A[k], 2)));
                }
            }

            // A и B на параллельных гранях.
            if (A[i] == 0 && B[i] == dim[i] || A[i] == dim[i] && B[i] == 0)
            {
                if (dim[j] - (A[j] + B[j]) > dim[k] - (A[k] + B[k]))
                {
                    (j, k) = (k, j);
                    back = true;
                }
                dist = A[j] + B[j];
                dist = Math.Min(2 * dim[j] - dist, dist);
                minDist = Math.Min(minDist, Math.Sqrt(Math.Pow(dim[i] + dist, 2) + Math.Pow(B[k] - A[k], 2)));
                if (back)
                {
                    (j, k) = (k, j);
                    back = false;
                }

                if (A[j] + B[k] < A[k] + B[j])
                {
                    (j, k) = (k, j);
                    back = true;
                }

                a = (dim[j] + dim[k] < 2 * (A[j] * B[k])) ? 1 : 0;
                b = (dim[k] + dim[j] < 2 * (A[k] + B[j])) ? 1 : 0;

                unB = new double[dim.Length];
                distA = new double[dim.Length];
                distB = new double[dim.Length];
                
                distA[j] = a * dim[j] + Math.Pow(-1, a + 2) * A[j];
                distB[k] = b * dim[k] + Math.Pow(-1, b + 2) * B[k];
                distA[k] = b * dim[k] + Math.Pow(-1, b + 2) * A[k];
                distB[j] = a * dim[j] + Math.Pow(-1, a + 2) * B[j];

                unB[j] = distA[j] + distB[k];
                unB[i] = dim[i] + distA[k] + distB[j];

                if (unB[i] / unB[j] * distA[j] < distA[k])
                {
                    minDist = Math.Min(minDist, Math.Sqrt(Math.Pow(distA[j], 2) + Math.Pow(distA[k], 2)) + Math.Sqrt(Math.Pow(unB[j] - distA[j], 2) + Math.Pow(unB[i] - distA[k], 2)));
                }
                else if (unB[i] / unB[j] * distA[j] > dim[k] - distB[k])
                {
                    minDist = Math.Min(minDist, Math.Sqrt(Math.Pow(distB[k], 2) + Math.Pow(distB[j], 2)) + Math.Sqrt(Math.Pow(unB[j] - distB[k], 2) + Math.Pow(unB[i] - distB[j], 2)));
                }
                else
                {
                    minDist = Math.Min(minDist, Math.Sqrt(Math.Pow(unB[j], 2) + Math.Pow(unB[i], 2)));
                }

                if (back)
                {
                    (j, k) = (k, j);
                    back = false;
                }
                return minDist;
            }
        }
        return -1;
    }
}