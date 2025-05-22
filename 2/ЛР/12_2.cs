using System;

unsafe class Program
{
    static void Main()
    {
        Console.Write("Введите размер массива: ");
        int size = int.Parse(Console.ReadLine());

        int* arr = stackalloc int[size];

        for (int i = 0; i < size; i++)
        {
            Console.Write($"Элемент {i + 1}: ");
            arr[i] = int.Parse(Console.ReadLine());
        }

        Console.WriteLine("\nПалиндромы в массиве:");
        for (int i = 0; i < size; i++)
        {
            if (IsPalindrome(arr[i]))
            {
                Console.WriteLine(arr[i]);
            }
        }
    }

    static bool IsPalindrome(int number)
    {
        if (number < 0) return false;
        
        int reversed = 0;
        int original = number;
        
        while (original > 0)
        {
            reversed = reversed * 10 + original % 10;
            original /= 10;
        }
        return number == reversed;
    }
}
