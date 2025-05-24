using System;
using System.Collections.Generic;
using System.Linq;

public class Phone
{
	public string Number { get; set; }
	public string Owner { get; set; }
	public int Year { get; set; }
	public string Operator { get; set; }
}

class Program
{
	static List<Phone> phones = new List<Phone>();

	static void Main()
	{
		while (true)
		{
			Console.Clear();
			Console.WriteLine("=== МЕНЮ ===");
			Console.WriteLine("1. Телефоны по оператору");
			Console.WriteLine("2. Телефоны по году выпуска");
			Console.WriteLine("3. Группировка по оператору");
			Console.WriteLine("4. Группировка по году выпуска");
			Console.WriteLine("5. Добавить новый телефон");
			Console.WriteLine("6. Выход");
			Console.Write("Выберите действие: ");

			switch (Console.ReadLine())
			{
				case "1": ShowByOperator(); break;
				case "2": ShowByYear(); break;
				case "3": GroupByOperator(); break;
				case "4": GroupByYear(); break;
				case "5": AddNewPhone(); break;
				case "6": return;
				default: Console.WriteLine("Неверный ввод!"); break;
			}
			Console.WriteLine("\nНажмите любую клавишу...");
			Console.ReadKey();
		}
	}

	static void AddNewPhone()
	{
		Console.WriteLine("\nДобавление нового телефона:");

		var newPhone = new Phone();

		Console.Write("Номер телефона: ");
		newPhone.Number = Console.ReadLine();

		Console.Write("Владелец: ");
		newPhone.Owner = Console.ReadLine();

		while (true)
		{
			Console.Write("Год выпуска: ");
			if (int.TryParse(Console.ReadLine(), out int year) && year > 1900 && year <= DateTime.Now.Year)
			{
				newPhone.Year = year;
				break;
			}
			Console.WriteLine("Некорректный год! Попробуйте снова.");
		}

		Console.Write("Оператор связи: ");
		newPhone.Operator = Console.ReadLine();

		phones.Add(newPhone);
		Console.WriteLine("\nТелефон успешно добавлен!");
	}
	static void ShowByOperator()
	{
		Console.Write("Введите оператора: ");
		string op = Console.ReadLine();
		
		var result = phones.Where(p => p.Operator.Equals(op, StringComparison.OrdinalIgnoreCase));
		
		PrintResult(result);
	}

	static void ShowByYear()
	{
		Console.Write("Введите год: ");
		if (!int.TryParse(Console.ReadLine(), out int year))
		{
			Console.WriteLine("Некорректный год!");
			return;
		}
		
		var result = phones.Where(p => p.Year == year);
		PrintResult(result);
	}

	static void GroupByOperator()
	{
		var groups = phones.GroupBy(p => p.Operator)
						  .OrderBy(g => g.Key);
		
		foreach (var group in groups)
		{
			Console.WriteLine($"\nОператор: {group.Key}");
			PrintResult(group);
		}
	}

	static void GroupByYear()
	{
		var groups = phones.GroupBy(p => p.Year)
						  .OrderByDescending(g => g.Key);
		
		foreach (var group in groups)
		{
			Console.WriteLine($"\nГод выпуска: {group.Key}");
			PrintResult(group);
		}
	}

	static void PrintResult(IEnumerable<Phone> result)
	{
		if (!result.Any())
		{
			Console.WriteLine("Данные не найдены!");
			return;
		}

		Console.WriteLine("\n{0,-15} {1,-20} {2,-10} {3}", 
			"Номер", "Владелец", "Год", "Оператор");
		
		foreach (var phone in result)
		{
			Console.WriteLine("{0,-15} {1,-20} {2,-10} {3}", 
				phone.Number, 
				phone.Owner, 
				phone.Year, 
				phone.Operator);
		}
	}
}
