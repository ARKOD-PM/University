    using System;

    class Program
    {
        static void FillArray(int[] array)
        {
            Console.WriteLine("Введите элементы массива: ");
            for (int i = 0; i < array.Length; i++)
            {
                while (true)
                {
                    Console.Write($"Элемент {i + 1}: ");
                    if (int.TryParse(Console.ReadLine(), out array[i]))
                        break;
                    Console.WriteLine("Пожалуйста, введите корректное целое число.");
                }
            }
        }

        static int FindMax(int[] array)
        {
            int max = array[0];
            foreach (var item in array)
            {
                if (item > max)
                {
                    max = item;
                }
            }
            return max;
        }

        static int FindMin(int[] array)
        {
            int min = array[0];
            foreach (var item in array)
            {
                if (item < min)
                {
                    min = item;
                }
            }
            return min;
        }

        static void Main()
        {
            Console.Write("Введите количество подмассивов: ");
            int n;

            while (!int.TryParse(Console.ReadLine(), out n) || n <= 0)
            {
                Console.WriteLine("Пожалуйста, введите положительное целое число.");
            }

            int[][] jaggedArray = new int[n][];

            for (int i = 0; i < n; i++)
            {
                Console.Write($"Введите размер массива {i + 1}: ");
                int size;

                while (!int.TryParse(Console.ReadLine(), out size) || size <= 0)
                {
                    Console.WriteLine("Пожалуйста, введите положительное целое число.");
                }

                jaggedArray[i] = new int[size];
                FillArray(jaggedArray[i]);
            }

            for (int i = 0; i < jaggedArray.Length; i++)
            {
                int max = FindMax(jaggedArray[i]);
                int min = FindMin(jaggedArray[i]);
                Console.WriteLine($"Массив {i + 1}, где (max, min) = ({max}, {min}).");
            }
        }
    }