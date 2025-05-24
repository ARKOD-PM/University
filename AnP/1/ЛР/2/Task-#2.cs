using System;

public class HelloWorld
{
    public static void Main()
    {
        int a = 1000, b = 7;
        Console.WriteLine((a + b + Math.Abs(a - b)) / 2);
        Console.WriteLine((a + b - Math.Abs(a - b)) / 2);
    }
}