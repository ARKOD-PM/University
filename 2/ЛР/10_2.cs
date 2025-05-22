using System;
using System.IO;
using System.Text.RegularExpressions;

public class FileProcessor
{
    public void ProcessFile(string inputPath, string outputPath)
    {
        try
        {
            string[] lines = File.ReadAllLines(inputPath);
            
            string[] filteredLines = FilterLines(lines);
            
            File.WriteAllLines(outputPath, filteredLines);
            
            Console.WriteLine($"Обработка завершена. Результат сохранен в: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка: {ex.Message}");
        }
    }

    private string[] FilterLines(string[] lines)
    {
        return Array.FindAll(lines, line => 
        {
            MatchCollection matches = Regex.Matches(line, @"\d+");
            
            foreach (Match match in matches)
            {
                if (int.TryParse(match.Value, out int number))
                {
                    if (number % 2 != 0)
                    {
                        return true;
                    }
                }
            }
            return false;
        });
    }
}

class Program
{
    static void Main()
    {
        var processor = new FileProcessor();
        
        Console.Write("Введите путь к исходному файлу: ");
        string input = Console.ReadLine();
        
        Console.Write("Введите путь для результата: ");
        string output = Console.ReadLine();

        processor.ProcessFile(input, output);
    }
}
