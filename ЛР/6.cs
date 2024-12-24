using System;
class Program
{
    static void Main()
    {
        Console.Write("Введите длину массива: ");
        int[] sequence = new int[Convert.ToInt16(Console.ReadLine())];
        int ends_3 = 0, d = 0, minimum = int.MaxValue, maximum = int.MinValue, pos_mn = 0, pos_mx = 0, tmp;
        bool progression = true;
        for (int i = 0; i < sequence.Length; i++)
        {
            Console.Write($"Введите {i + 1} элемент массива: ");
            sequence[i] = Convert.ToInt32(Console.ReadLine());
            if (Math.Abs(sequence[i]) % 10 == 3)
            {
                ends_3++;
            }
            if (i == 1)
            {
                d = sequence[i] - sequence[i - 1];
            }
            else if (i > 1 && (d <= 0 || sequence[i] - sequence[i - 1] != d))
            {
                progression = false;
            }
            if (sequence[i] < minimum)
            {
                minimum = sequence[i];
                pos_mn = i;
            }
            else if (sequence[i] > maximum)
            {
                maximum = sequence[i];
                pos_mx = i;
            }
        }
        var str = string.Join(", ", sequence);
        Console.WriteLine($"Исходная последовательность: [{str}]");
        tmp = sequence[pos_mx];
        sequence[pos_mx] = sequence[pos_mn];
        sequence[pos_mn] = tmp;
        str = string.Join(", ", sequence);
        Console.WriteLine($"Количество элементов, оканчивающихся на 3: {ends_3}\n" +
            $"Является ли последовательность возрастающей арифметической прогрессией: {progression}\n" +
            $"Изменённая последовательность: [{str}]");
    }
}