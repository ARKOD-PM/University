using System;
using System.Collections.Generic;

public class Car
{
	public string Brand { get; }
	public string Owner { get; }
	public int Year { get; }
	public bool IsWashed { get; set; }

	public Car(string brand, string owner, int year)
	{
		Brand = brand;
		Owner = owner;
		Year = year;
		IsWashed = false;
	}
}

public class GarageDatabase
{
	private List<Car> _cars = new List<Car>();

	public void AddCar(Car car) => _cars.Add(car);
	public IEnumerable<Car> GetAllCars() => _cars;
	public IEnumerable<Car> GetDirtyCars() => _cars.FindAll(c => !c.IsWashed);
}

public class CarWashService
{
	public delegate void WashHandler(Car car);
	
	public void WashCar(Car car)
	{
		if (car.IsWashed) return;
		
		car.IsWashed = true;
		Console.WriteLine($"> {car.Brand} ({car.Year}) владельца {car.Owner} помыта");
	}
}

class Program
{
	static void Main()
	{
		var db = new GarageDatabase();
		var washer = new CarWashService();
		CarWashService.WashHandler washDelegate = washer.WashCar;

		while(true)
		{
			Console.Clear();
			Console.WriteLine0("=== АВТОМОЙКА ===");
			Console.WriteLine("1. Добавить машину");
			Console.WriteLine("2. Помыть все грязные машины");
			Console.WriteLine("3. Показать все машины");
			Console.WriteLine("4. Выход");
			Console.Write("Выберите действие: ");

			switch(Console.ReadLine())
			{
				case "1": AddCarMenu(db); break;
				case "2": WashAllDirtyCars(db, washDelegate); break;
				case "3": ShowAllCars(db); break;
				case "4": return;
				default: ShowError("Неверный ввод!"); break;
			}
			Console.WriteLine("\nНажмите любую клавишу...");
			Console.ReadKey();
		}
	}

	static void AddCarMenu(GarageDatabase db)
	{
		Console.Write("\nМарка: ");
		string brand = Console.ReadLine();

		Console.Write("Владелец: ");
		string owner = Console.ReadLine();

		int year;
		while(true)
		{
			Console.Write("Год выпуска: ");
			if(int.TryParse(Console.ReadLine(), out year)) break;
			ShowError("Некорректный год!");
		}

		db.AddCar(new Car(brand, owner, year));
		Console.WriteLine("\nМашина добавлена!");
	}

	static void WashAllDirtyCars(GarageDatabase db, CarWashService.WashHandler wash)
	{
		var dirtyCars = new List<Car>(db.GetDirtyCars());
		
		if(dirtyCars.Count == 0)
		{
			ShowError("Все машины уже чистые!");
			return;
		}

		Console.WriteLine("\nНачинаем мойку:");
		dirtyCars.ForEach(wash.Invoke);
	}

	static void ShowAllCars(GarageDatabase db)
	{
		Console.WriteLine("\nСписок всех машин:");
		Console.WriteLine("----------------------------------------");
		
		int counter = 1;
		foreach(var car in db.GetAllCars())
		{
			Console.WriteLine($"{counter}. {car.Brand.ToUpper()}");
			Console.WriteLine($"   Владелец: {car.Owner}");
			Console.WriteLine($"   Год выпуска: {car.Year}");
			Console.WriteLine($"   Состояние: {(car.IsWashed ? "Чистая" : "Требуется мойка")}");
			Console.WriteLine("----------------------------------------");
			counter++;
		}
		
	}
	static void ShowError(string message)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"\n{message}");
		Console.ResetColor();
	}
}
