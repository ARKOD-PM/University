using System;
using System.Globalization;
using System.IO;

class Program
{
    static void Main()
    {
        RunTests(); // Запуск тестов
    }

    static void RunTests()
    {
        var testFiles = Directory.GetFiles("tests", "input*.txt");

        foreach (var inputFilePath in testFiles)
        {
            string outputFilePath = inputFilePath.Replace("input", "output");

            if (File.Exists(outputFilePath))
            {
                var inputs = File.ReadAllLines(inputFilePath);
                var expectedOutput = File.ReadAllText(outputFilePath).Trim();

                string output = CalculateMinimumMilkCost(inputs);
                Console.WriteLine($"Testing {Path.GetFileName(inputFilePath)} - Expected: {expectedOutput}, Got: {output}");
                Console.WriteLine(output == expectedOutput ? "Test passed!" : "Test failed.");
            }
            else
            {
                Console.WriteLine($"Output file for {inputFilePath} is missing.");
            }
        }
    }

    static string CalculateMinimumMilkCost(string[] input)
    {
        int N = int.Parse(input[0]);

        double minCostPerLiter = double.MaxValue;
        int bestCompanyIndex = -1;

        for (int i = 1; i <= N; i++)
        {
            var data = input[i].Split();
            int Xi1 = int.Parse(data[0]);
            int Yi1 = int.Parse(data[1]);
            int Zi1 = int.Parse(data[2]);
            int Xi2 = int.Parse(data[3]);
            int Yi2 = int.Parse(data[4]);
            int Zi2 = int.Parse(data[5]);
            double Ci1 = double.Parse(data[6], CultureInfo.InvariantCulture);
            double Ci2 = double.Parse(data[7], CultureInfo.InvariantCulture);

            double volume1 = (Xi1 * Yi1 * Zi1) / 1000.0;
            double volume2 = (Xi2 * Yi2 * Zi2) / 1000.0;

            int surfaceArea1 = 2 * (Xi1 * Yi1 + Xi1 * Zi1 + Yi1 * Zi1);
            int surfaceArea2 = 2 * (Xi2 * Yi2 + Xi2 * Zi2 + Yi2 * Zi2);

            double totalCostDiff = Ci1 / volume1 - Ci2 / volume2;
            double surfaceAreaDiff = (double)surfaceArea1 / volume1 - (double)surfaceArea2 / volume2;

            double materialCostPerUnitArea = totalCostDiff / surfaceAreaDiff;
            double costPerLiterMilk1 = Ci1 / volume1 - materialCostPerUnitArea * surfaceArea1 / volume1;
            double costPerLiterMilk2 = Ci2 / volume2 - materialCostPerUnitArea * surfaceArea2 / volume2;

            double costPerLiterMilk = Math.Min(costPerLiterMilk1, costPerLiterMilk2);

            if (costPerLiterMilk < minCostPerLiter)
            {
                minCostPerLiter = costPerLiterMilk;
                bestCompanyIndex = i;
            }
        }

        return $"{bestCompanyIndex} {minCostPerLiter:F2}";
    }
}