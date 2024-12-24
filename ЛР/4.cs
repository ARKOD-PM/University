using System;

class SequenceAnalyzer
{
    static void Main()
    {
        Console.Write("Введите длину последовательности: ");
        
        if (!int.TryParse(Console.ReadLine(), out int range) || range <= 0)
        {
            Console.WriteLine("Ошибка: введите корректное положительное число.");
            return;
        }

        int previousEven = 0;
        int minOnesSequence = int.MaxValue;
        int maxSameEvenSequence = 0;
        int currentOnesCount = 0;
        int currentEvenCount = 0;
        int currentEvenSum = 0;
        int maxEvenSum = int.MinValue;

        for (int i = 0; i < range; i++)
        {
            Console.Write($"Введите {i + 1} число: ");
            
            if (!int.TryParse(Console.ReadLine(), out int number))
            {
                Console.WriteLine("Ошибка: введите корректное число.");
                i--;
                continue;
            }

            if (number == 1)
            {
                currentOnesCount++;
            }
            else
            {
                if (currentOnesCount > 0)
                {
                    minOnesSequence = Math.Min(minOnesSequence, currentOnesCount);
                    currentOnesCount = 0;
                }
            }

            if (number % 2 == 0)
            {
                if (number != previousEven)
                {
                    currentEvenCount = 0;
                }

                currentEvenCount++;
                maxSameEvenSequence = Math.Max(maxSameEvenSequence, currentEvenCount);
                
                previousEven = number;

                currentEvenSum += number;
                currentEvenSum = Math.Max(currentEvenSum, number)

                maxEvenSum = Math.Max(maxEvenSum, currentEvenSum);
            }
            else
            {
                currentEvenSum = 0;
            }
        }

        if (currentOnesCount > 0)
        {
            minOnesSequence = Math.Min(minOnesSequence, currentOnesCount);
        }

        if (minOnesSequence == int.MaxValue)
        {
            minOnesSequence = 0;
        }

        if (maxEvenSum == int.MinValue)
        {
            maxEvenSum = 0;
        }

        Console.WriteLine($"Минимальная мощность последовательности из единиц: {minOnesSequence}");
        Console.WriteLine($"Максимальная мощность последовательности из одинаковых чётных: {maxSameEvenSequence}");
        Console.WriteLine($"Максимальная сумма последовательности чётных элементов: {maxEvenSum}");
    }
}
