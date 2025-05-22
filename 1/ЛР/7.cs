using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Write("Введите размер n для двумерного массива n x n: ");
        int n = Convert.ToInt32(Console.ReadLine());
        int[,] array = new int[n, n];
        Random rand = new Random();

        // Заполнение массива случайными числами от 0 до 9
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                array[i, j] = rand.Next(0, 10);
            }
        }

        // Печать изначального массива
        Console.WriteLine("Изначальный массив:");
        PrintArray(array);

        // 1. Найти пары столбцов с одинаковыми элементами
        FindIdenticalColumns(array);

        // 2. Сортировать строки по убыванию количества нулей
        SortRowsByZeroCount(array);

        // Печать массива после сортировки строк
        Console.WriteLine("Массив после сортировки строк:");
        PrintArray(array);

        // 3. Поменять местами максимальный и минимальный элементы
        SwapMaxMin(array);

        // Печать окончательного массива
        Console.WriteLine("Окончательный массив после замены максимального и минимального элементов:");
        PrintArray(array);
    }

    static void PrintArray(int[,] array)
    {
        int n = array.GetLength(0);
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                Console.Write(array[i, j] + "\t");
            }
            Console.WriteLine();
        }
    }

    static void FindIdenticalColumns(int[,] array)
    {
        int n = array.GetLength(0);
        List<Tuple<int, int>> identicalColumns = new List<Tuple<int, int>>();

        for (int col1 = 0; col1 < n - 1; col1++)
        {
            for (int col2 = col1 + 1; col2 < n; col2++)
            {
                bool identical = true;
                for (int row = 0; row < n; row++)
                {
                    if (array[row, col1] != array[row, col2])
                    {
                        identical = false;
                        break;
                    }
                }
                if (identical)
                {
                    identicalColumns.Add(new Tuple<int, int>(col1, col2));
                }
            }
        }

        Console.WriteLine("Пары столбцов с одинаковыми элементами:");
        foreach (var pair in identicalColumns)
        {
            Console.WriteLine($"Столбец {pair.Item1} и Столбец {pair.Item2}");
        }
    }

    static void SortRowsByZeroCount(int[,] array)
    {
        int n = array.GetLength(0);

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (CountZeros(array, j) < CountZeros(array, j + 1))
                {
                    // Меняем строки местами
                    for (int k = 0; k < n; k++)
                    {
                        int temp = array[j, k];
                        array[j, k] = array[j + 1, k];
                        array[j + 1, k] = temp;
                    }
                }
            }
        }
    }

    static int CountZeros(int[,] array, int row)
    {
        int zeroCount = 0;
        int n = array.GetLength(1);
        for (int i = 0; i < n; i++)
        {
            if (array[row, i] == 0)
            {
                zeroCount++;
            }
        }
        return zeroCount;
    }

    static void SwapMaxMin(int[,] array)
    {
        int n = array.GetLength(0);
        int maxVal = int.MinValue, minVal = int.MaxValue;
        int maxRow = 0, maxCol = 0, minRow = 0, minCol = 0;
        // Находим максимальные и минимальные значения и их позиции
        for (int i = 0; i < n; i++)
        {
            for (int j = 0; j < n; j++)
            {
                if (array[i, j] > maxVal)
                {
                    maxVal = array[i, j];
                    maxRow = i;
                    maxCol = j;
                }
                if (array[i, j] < minVal)
                {
                    minVal = array[i, j];
                    minRow = i;
                    minCol = j;
                }
            }
        }

        // Меняем местами максимальный и минимальный элементы
        int temp = array[maxRow, maxCol];
        array[maxRow, maxCol] = array[minRow, minCol];
        array[minRow, minCol] = temp;
    }
}