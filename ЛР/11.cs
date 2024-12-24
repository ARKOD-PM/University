using System;

public class MyClass
{
    private int _value1;
    private int _value2;

    // Конструктор без параметров
    public MyClass()
    {
        _value1 = 0;
        _value2 = 0;
    }

    // Конструктор с одним параметром
    public MyClass(int value1)
    {
        _value1 = value1;
        _value2 = 0;
    }

    // Конструктор с двумя параметрами
    public MyClass(int value1, int value2)
    {
        _value1 = value1;
        _value2 = value2;
    }

    // Метод для сложения двух полей
    public int Sum()
    {
        return _value1 + _value2;
    }

    // Метод для разности первого и второго поля
    public int Difference()
    {
        return _value1 - _value2;
    }

    // Метод для произведения двух полей
    public int Product()
    {
        return _value1 * _value2;
    }

    // Метод для деления первого поля на второе
    public double Division()
    {
        if (_value2 == 0)
        {
            Console.WriteLine("Ошибка: деление на ноль невозможно.");
            return double.NaN; // Возвращаем "не число" при делении на ноль
        }
        return (double)_value1 / _value2;
    }
}

class Program
{
    static void Main()
    {
        // Создание объектов с использованием разных конструкторов
        MyClass obj1 = new MyClass();
        MyClass obj2 = new MyClass(5);
        MyClass obj3 = new MyClass(10, 2);

        // Выполнение методов для первого объекта
        Console.WriteLine("Объект 1:");
        Console.WriteLine("Сумма: " + obj1.Sum());
        Console.WriteLine("Разность: " + obj1.Difference());
        Console.WriteLine("Произведение: " + obj1.Product());
        Console.WriteLine("Деление: " + obj1.Division());

        // Выполнение методов для второго объекта
        Console.WriteLine("\nОбъект 2:");
        Console.WriteLine("Сумма: " + obj2.Sum());
        Console.WriteLine("Разность: " + obj2.Difference());
        Console.WriteLine("Произведение: " + obj2.Product());
        Console.WriteLine("Деление: " + obj2.Division());

        // Выполнение методов для третьего объекта
        Console.WriteLine("\nОбъект 3:");
        Console.WriteLine("Сумма: " + obj3.Sum());
        Console.WriteLine("Разность: " + obj3.Difference());
        Console.WriteLine("Произведение: " + obj3.Product());
        Console.WriteLine("Деление: " + obj3.Division());
    }
}