using System;
using System.Collections.Generic;
using System.Linq;

class Phone
{
	public string Number { get; set; }
	public string Operator { get; set; }

	public Phone(string number, string operatorName)
	{
		Number = number;
		Operator = operatorName;
	}
}

class Program
{
	static void Main()
	{
	  Console.WriteLine("Введите количество телефонов:");
	int n = int.Parse(Console.ReadLine());

	List<Phone> phones = new List<Phone>();
	for (int i = 0; i < n; i++)
	{
		Console.Write($"Номер {i+1}: ");
		string num = Console.ReadLine();
	
		Console.Write($"Оператор {i+1}: ");
		string op = Console.ReadLine();
	
		phones.Add(new Phone(num, op));
	} 
		Dictionary<string, int> operatorStats = new Dictionary<string, int>();

		foreach (Phone phone in phones)
		{
			if (operatorStats.ContainsKey(phone.Operator))
				operatorStats[phone.Operator]++;
			else
				operatorStats[phone.Operator] = 1;
		}

		Console.WriteLine("Статистика операторов:");
		foreach (var entry in operatorStats.OrderByDescending(e => e.Value))
		{
			Console.WriteLine($"{entry.Key}: {entry.Value}");
		}
	}
}
