using System;
using System.Collections.Generic;
using System.Linq;

public class Product
{
	public int Id { get; set; }
	public string Name { get; set; }
}

public class Supplier
{
	public int Id { get; set; }
	public string Name { get; set; }
}

public class ProductMovement
{
	public int ProductId { get; set; }
	public int? SupplierId { get; set; }
	public string OperationType { get; set; }
	public int Quantity { get; set; }
	public decimal Price { get; set; }
	public DateTime Date { get; set; }
}

class Program
{
	// HardCode для проверки функционала пойдёт.
	static List<Product> products = new List<Product>
	{
		new Product { Id = 1, Name = "Молоко" },
		new Product { Id = 2, Name = "Хлеб" }
	};

	static List<Supplier> suppliers = new List<Supplier>
	{
		new Supplier { Id = 1, Name = "Молочный комбинат" },
		new Supplier { Id = 2, Name = "Хлебозавод №1" }
	};

	static List<ProductMovement> movements = new List<ProductMovement>
	{
		new ProductMovement { ProductId = 1, SupplierId = 1, OperationType = "Поставка", 
			Quantity = 100, Price = 50, Date = new DateTime(2024, 1, 1) },
		new ProductMovement { ProductId = 1, OperationType = "Продажа", 
			Quantity = 80, Price = 80, Date = new DateTime(2024, 1, 5) },
		new ProductMovement { ProductId = 1, OperationType = "Списание", 
			Quantity = 10, Price = 0, Date = new DateTime(2024, 1, 10) },
		new ProductMovement { ProductId = 2, SupplierId = 2, OperationType = "Поставка", 
			Quantity = 200, Price = 30, Date = new DateTime(2024, 1, 2) },
		new ProductMovement { ProductId = 2, OperationType = "Продажа", 
			Quantity = 150, Price = 40, Date = new DateTime(2024, 1, 6) }
	};

	static void Main()
	{
		while (true)
		{
			Console.Clear();
			Console.WriteLine("=== МЕНЮ МАГАЗИНА ===");
			Console.WriteLine("1. Выручка за период");
			Console.WriteLine("2. Остатки на складе");
			Console.WriteLine("3. Списанные товары");
			Console.WriteLine("4. Поставки по поставщикам");
			Console.WriteLine("5. Продажи по дате");
			Console.WriteLine("6. Выход");
			Console.Write("Выберите действие: ");

			switch (Console.ReadLine())
			{
				case "1": ShowRevenueByPeriod(); break;
				case "2": ShowStockBalance(); break;
				case "3": ShowWriteOffs(); break;
				case "4": ShowSuppliesBySupplier(); break;
				case "5": ShowSalesByDate(); break;
				case "6": return;
				default: Console.WriteLine("Неверный ввод!"); break;
			}
			Console.WriteLine("\nНажмите любую клавишу...");
			Console.ReadKey();
		}
	}

	static void ShowRevenueByPeriod()
	{
		Console.Write("Введите начальную дату (гггг-мм-дд): ");
		DateTime start = DateTime.Parse(Console.ReadLine());
		Console.Write("Введите конечную дату (гггг-мм-дд): ");
		DateTime end = DateTime.Parse(Console.ReadLine());

		var revenue = movements
			.Where(m => m.OperationType == "Продажа" && 
				   m.Date >= start && 
				   m.Date <= end)
			.Sum(m => m.Quantity * m.Price);

		Console.WriteLine($"Выручка (дубли): {revenue}");
	}

	static void ShowStockBalance()
	{
		var stock = products.Select(p => new
		{
			Product = p.Name,
			Balance = movements.Where(m => m.ProductId == p.Id)
				.Sum(m => m.OperationType == "Поставка" ? m.Quantity : -m.Quantity)
		});

		Console.WriteLine("\nОстатки на складе:");
		foreach (var item in stock)
			Console.WriteLine($"{item.Product}: {item.Balance} шт.");
	}

	static void ShowWriteOffs()
	{
		var writeOffs = movements
			.Where(m => m.OperationType == "Списание")
			.Join(products,
				m => m.ProductId,
				p => p.Id,
				(m, p) => new { p.Name, m.Quantity, m.Date })
			.OrderBy(x => x.Name);

		Console.WriteLine("\nСписанные товары:");
		foreach (var item in writeOffs)
			Console.WriteLine($"{item.Name} - {item.Quantity} шт. ({item.Date:dd.MM.yyyy})");
	}

	static void ShowSuppliesBySupplier()
	{
		var supplies = movements
			.Where(m => m.OperationType == "Поставка")
			.Join(suppliers,
				m => m.SupplierId.Value,
				s => s.Id,
				(m, s) => new { s.Name, m.ProductId, m.Quantity })
			.Join(products,
				s => s.ProductId,
				p => p.Id,
				(s, p) => new { Supplier = s.Name, Product = p.Name, s.Quantity })
			.OrderBy(x => x.Supplier)
			.ThenBy(x => x.Product);

		Console.WriteLine("\nПоставки по поставщикам:");
		foreach (var item in supplies)
			Console.WriteLine($"{item.Supplier} -> {item.Product}: {item.Quantity} шт.");
	}

	static void ShowSalesByDate()
	{
		var sales = movements
			.Where(m => m.OperationType == "Продажа")
			.Join(products,
				m => m.ProductId,
				p => p.Id,
				(m, p) => new { p.Name, m.Quantity, m.Price, m.Date })
			.OrderBy(x => x.Date)
			.ThenBy(x => x.Name);

		Console.WriteLine("\nПродажи по дате:");
		foreach (var item in sales)
			Console.WriteLine($"{item.Date:dd.MM.yyyy} {item.Name} - " +
				$"{item.Quantity} шт. по {item.Price} (дублей)");
	}
}
