using System;

namespace Project
{
    class Program
    {
        static void Main()
        {
            // Объявление переменных
            int n, input;
            int max = int.MinValue, secondMax = int.MinValue;
            int localMaxCount = 0, endsWithFiveCount = 0;
            bool allOdd = true;

            // Ввод количества чисел
            Console.WriteLine("Введите значение n: ");
            n = Convert.ToInt32(Console.ReadLine());

            // Переменные для хранения предыдущих значений
            int previous = int.MinValue, beforePrevious = int.MinValue;

            // Цикл для ввода и обработки чисел
            for (int i = 0; i < n; i++)
            {
                Console.WriteLine($"Введите {i + 1} число: ");
                input = Convert.ToInt32(Console.ReadLine());

                // Проверка на наличие четного числа
                if (input % 2 == 0)
                {
                    allOdd = false;
                }

                // Проверка чисел, заканчивающихся на 5
                if (Math.Abs(input) % 10 == 5)
                {
                    endsWithFiveCount++;
                }

                // Определение максимального и второго максимального значения
                if (input > max)
                {
                    secondMax = max;
                    max = input;
                }
                else if (input > secondMax)
                {
                    secondMax = input;
                }

                // Подсчет локальных максимумов
                if (i > 1 && beforePrevious <= previous && previous >= input)
                {
                    localMaxCount++;
                }

                // Обновление предыдущих значений
                beforePrevious = previous;
                previous = input;
            }

            // Вывод результатов
            Console.WriteLine($"Локальные максимумы: {localMaxCount}");
            Console.WriteLine($"Все числа нечетные: {allOdd}");
            Console.WriteLine($"Второе максимальное значение: {secondMax}");
            Console.WriteLine($"Чисел, заканчивающихся на 5: {endsWithFiveCount}");
        }
    }
}