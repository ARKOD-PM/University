using System;

class Program
{
    static void Main()
    {
        int even = 0;
        bool odd = true;
        while (true)
        {
            Console.Write("Введите число: ");
            int input = Convert.ToInt32(Console.ReadLine());
            if (input <= 0)
            {
                break;
            }
            while (input > 0)
            {
                int num = input % 10;
                if (num % 2 == 0)
                {
                    odd = false;
                    even = even * 10 + num;
                }
                input /= 10;
            }
        }
        if (odd)
        {
            Console.WriteLine("В числе отстуствую чётные цифры.");
        }
        else
        {
            Console.WriteLine($"Вывод: {even}");
        }
    }
}