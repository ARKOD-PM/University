using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;

class Program
{
    static void Main()
    {
        Console.Write("Введите строку: ");
        string? inputLine = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(inputLine))
        {
            Console.WriteLine("Ошибка: введена пустая строка.");
            return;
        }
        string[] words = inputLine.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        Console.WriteLine("1) " + string.Join(' ', words));
        int countCertainWords = 0;
        int countCertain2Words = 0;
        char[] vowels = {'a', 'e', 'i', 'o', 'u', 'y'};
        Console.Write("4)");
        foreach (string word in words)
        {
            string lowerWord = word.ToLower();
            if (word.Length % 2 != 0 && lowerWord[0] == lowerWord[lowerWord.Length - 1])
            {
                countCertainWords++;
            }
            if (word.Length >= 2)
            {
                countCertain2Words += 1;
            }
            for (int i = 1; i < lowerWord.Length - 1; i += 2)
            {
                if (!vowels.Contains(lowerWord[i]))
                {
                    --countCertain2Words;
                    break;
                }
            }
            if (lowerWord.Contains('a'))
            {
                Console.Write(' ' + word);
            }
        }
        Console.WriteLine("\n" + "2) " + countCertain2Words);
        Console.WriteLine("3) " + countCertainWords);
    }
}