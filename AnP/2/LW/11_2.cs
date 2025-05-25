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
	static List<Product> products = new List<Product>();
	static List<Supplier> suppliers = new List<Supplier>();
	static List<ProductMovement> movements = new List<ProductMovement>();

	static void Main()
	{
		InitializeSampleData(); // Hard Code, потому что легче.
		
		while (true)
		{
			Console.Clear();
			Console.WriteLine("=== МЕНЮ УПРАВЛЕНИЯ СКЛАДОМ ===");
			Console.WriteLine("1. Остатки товаров");
			Console.WriteLine("2. Поставки по поставщикам");
			Console.WriteLine("3. Продажи по датам");
			Console.WriteLine("4. Выручка от продаж");
			Console.WriteLine("5. Лучший поставщик");
			Console.WriteLine("6. Выход");
			Console.Write("Выберите действие: ");

			switch (Console.ReadLine())
			{
				case "1": ShowStock(); break;
				case "2": ShowSuppliesBySupplier(); break;
				case "3": ShowSalesByDate(); break;
				case "4": ShowTotalRevenue(); break;
				case "5": ShowTopSupplier(); break;
				case "6": return;
				default: Console.WriteLine("Неверный ввод!"); break;
			}
			Console.WriteLine("\nНажмите любую клавишу...");
			Console.ReadKey();
		}
	}

	static void InitializeSampleData()
	{
		products.Add(new Product { Id = 1, Name = "Ноутбук" });
		products.Add(new Product { Id = 2, Name = "Смартфон" });

		suppliers.Add(new Supplier { Id = 1, Name = "TechSupplier Inc." });
		suppliers.Add(new Supplier { Id = 2, Name = "GadgetWorld Ltd." });

		movements.Add(new ProductMovement 
		{ 
			ProductId = 1, 
			SupplierId = 1, 
			OperationType = "Поставка", 
			Quantity = 50, 
			Price = 50000, 
			Date = DateTime.Parse("2024-01-10") 
		});

		movements.Add(new ProductMovement 
		{ 
			ProductId = 1, 
			OperationType = "Продажа", 
			Quantity = 20, 
			Price = 70000, 
			Date = DateTime.Parse("2024-01-15") 
		});
	}

	static void ShowStock()
	{
		Console.WriteLine("\nОстатки товаров:");
		foreach (var product in products)
		{
			var stock = movements
				.Where(m => m.ProductId == product.Id)
				.Sum(m => m.OperationType == "Поставка" ? m.Quantity : -m.Quantity);

			Console.WriteLine($"{product.Name}: {stock} шт.");
		}
	}

	static void ShowSuppliesBySupplier()
	{
		var supplies = movements
			.Where(m => m.OperationType == "Поставка")
			.GroupBy(m => m.SupplierId)
			.Select(g => new {
				Supplier = suppliers.First(s => s.Id == g.Key.Value).Name,
				Total = g.Sum(m => m.Quantity)
			});

		Console.WriteLine("\nПоставки по поставщикам:");
		foreach (var group in supplies)
		{
			Console.WriteLine($"{group.Supplier}: {group.Total} шт.");
		}
	}

	static void ShowSalesByDate()
	{
		var sales = movements
			.Where(m => m.OperationType == "Продажа")
			.GroupBy(m => m.Date.Date)
			.OrderBy(g => g.Key);

		Console.WriteLine("\nПродажи по датам:");
		foreach (var group in sales)
		{
			Console.WriteLine($"{group.Key:dd.MM.yyyy}:");
			foreach (var sale in group)
			{
				var product = products.First(p => p.Id == sale.ProductId);
				Console.WriteLine($"  {product.Name} - {sale.Quantity} шт. по {sale.Price} (дублей)");
			}
		}
	}

	static void ShowTotalRevenue()
	{
		var revenue = movements
			.Where(m => m.OperationType == "Продажа")
			.Sum(m => m.Quantity * m.Price);

		Console.WriteLine($"\nОбщая выручка (дубли): {revenue}");
	}

	static void ShowTopSupplier()
	{
		var topSupplier = movements
			.Where(m => m.OperationType == "Поставка")
			.GroupBy(m => m.SupplierId)
			.Select(g => new {
				SupplierId = g.Key.Value,
				Total = g.Sum(m => m.Quantity)
			})
			.OrderByDescending(g => g.Total)
			.FirstOrDefault();

		if (topSupplier != null)
		{
			var supplier = suppliers.First(s => s.Id == topSupplier.SupplierId);
			Console.WriteLine($"\nЛучший поставщик: {supplier.Name} ({topSupplier.Total} шт.)");
		}
		else
		{
			Console.WriteLine("\nДанные о поставках отсутствуют");
		}
	}
}
