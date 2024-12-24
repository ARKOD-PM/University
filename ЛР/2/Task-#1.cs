using System;

public class HelloWorld
{
    public static void Main()
    {
        int a = 14, b = 12;
        a+=b;
        b=a-b;
        a-=b;
        Console.WriteLine(a+" "+b);
    }
}
