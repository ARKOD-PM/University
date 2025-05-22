// На вход подаётся строка, состоящая из заглавных букв латинского алфавита.
//    1) Необходимо определить максимальную длину подстроки, состоящую из последовательности элементов x, y, z (в этом порядке), при этом допускается неполная их комбинация;
//    2) Дана строка, состоящая из латинских букв. Необходимо определить символ, который реже всего встречается в образце A*B, где * - искомый символ (присутствовать должен). Если таких символов несколько, выдать все.

using System;
class Program
{
    public static int findMin(int[] arr)
    {
        if (arr.Length == 0)
        {
            throw new Exception("Array is empty");
        }

        int min = int.MaxValue;
        foreach (var i in arr)
        {
            if (i < min && i != 0)
            {
                min = i;
            }
        }
        return min;
    }

    static void Main()
    {
        Console.Write("Введите строку: ");
        string? inputLine = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(inputLine))
        {
            Console.WriteLine("Ошибка: введена пустая строка.");
            return;
        }
        char[] elements = { 'x', 'y', 'z' };
        int maxLength = 0;
        int curLength = 0;
        foreach (char element in inputLine)
        {
            if (curLength == 0 && element == elements[0] || element == elements[curLength % 3])
            {
                curLength++;
                maxLength = Math.Max(maxLength, curLength);
            }
            else
            {
                curLength = 0;
            }
        }
        Console.WriteLine("1) " + maxLength);
        inputLine = inputLine.ToLower();
        char[] inputElements = new char[26];
        int[] inputElementsCount = new int[26];
        int currentElement = 0;
        int index = -1;
        for (int i = 0; i < inputLine.Length - 2; i++)
        {
            if (inputLine[i] == 'a' && inputLine[i + 2] == 'b')
            {
                index = Array.IndexOf(inputElements, inputLine[i + 1]);
                if (index == -1)
                {
                    
                    inputElements[currentElement] = inputLine[i + 1];
                    inputElementsCount[currentElement]++;
                    currentElement++;
                }
                else
                {
                    ++inputElementsCount[index];
                }
            }
        }
        int[] indexMinCount = new int[50];
        int minCount = findMin(inputElementsCount);
        Console.Write("2) ");
        for (int i = 0; i < inputElementsCount.Length; i++)
        {
            if (minCount == inputElementsCount[i])
            {
                Console.Write(inputElements[i] + " ");
            }
        }
    }
}
