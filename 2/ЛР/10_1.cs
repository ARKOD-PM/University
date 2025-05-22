using System;
using System.Collections.Generic;
using System.Linq;

public struct Book
{
	public int Id;
	public string Author;
	public string Title;
	public int Year;
	public string Publisher;
}

public class LibraryRecord
{
	public int BookId;
	public DateTime IssueDate;
	public DateTime? ReturnDate;
}

public class LibraryDatabase
{
	private List<Book> _books = new List<Book>();
	private List<LibraryRecord> _records = new List<LibraryRecord>();
	private int _nextBookId = 1;

	public void AddBook(string author, string title, int year, string publisher)
	{
		_books.Add(new Book
		{
			Id = _nextBookId++,
			Author = author,
			Title = title,
			Year = year,
			Publisher = publisher
		});
	}

	public void IssueBook(int bookId)
	{
		if (!_books.Any(b => b.Id == bookId))
			throw new ArgumentException("Книга с указанным ID не найдена");

		if (_records.Any(r => r.BookId == bookId && r.ReturnDate == null))
			throw new InvalidOperationException("Книга уже выдана");

		_records.Add(new LibraryRecord
		{
			BookId = bookId,
			IssueDate = DateTime.Now,
			ReturnDate = null
		});
	}

	public void ReturnBook(int bookId)
	{
		var record = _records.FirstOrDefault(r => 
			r.BookId == bookId && r.ReturnDate == null);

		if (record == null)
			throw new InvalidOperationException("Книга не была выдана");

		record.ReturnDate = DateTime.Now;
	}

	public IEnumerable<Book> GetNeverIssuedBooks()
	{
		return _books.Where(b => 
			!_records.Any(r => r.BookId == b.Id));
	}

	public IEnumerable<Book> GetNotReturnedBooks()
	{
		return from book in _books
			   join record in _records 
				   on book.Id equals record.BookId
			   where record.ReturnDate == null
			   select book;
	}

	public void PrintBooks(IEnumerable<Book> books)
	{
		Console.WriteLine("{0,-5} {1,-25} {2,-30} {3,-6} {4}",
			"ID", "Автор", "Название", "Год", "Издательство");
		
		foreach (var book in books)
		{
			Console.WriteLine("{0,-5} {1,-25} {2,-30} {3,-6} {4}",
				book.Id,
				book.Author,
				book.Title,
				book.Year,
				book.Publisher);
		}
	}
}

class Program
{
	private static LibraryDatabase _db = new LibraryDatabase();

	static void Main()
	{
		while (true)
		{
			Console.Clear();
			Console.WriteLine("=== БИБЛИОТЕЧНАЯ СИСТЕМА ===");
			Console.WriteLine("1. Добавить новую книгу");
			Console.WriteLine("2. Выдать книгу читателю");
			Console.WriteLine("3. Принять возврат книги");
			Console.WriteLine("4. Никогда не выданные книги");
			Console.WriteLine("5. Текущие должники");
			Console.WriteLine("6. Выход");
			Console.Write("Выберите действие: ");

			switch (Console.ReadLine())
			{
				case "1": AddBookMenu(); break;
				case "2": IssueBookMenu(); break;
				case "3": ReturnBookMenu(); break;
				case "4": ShowNeverIssued(); break;
				case "5": ShowNotReturned(); break;
				case "6": return;
				default: ShowError("Неверный пункт меню!"); break;
			}
			Console.WriteLine("\nНажмите любую клавишу...");
			Console.ReadKey();
			
		}
	}

	static void AddBookMenu()
	{
		Console.Write("\nАвтор: ");
		string author = Console.ReadLine();

		Console.Write("Название: ");
		string title = Console.ReadLine();

		int year = ReadInt("Год издания: ", 1000, DateTime.Now.Year);
		
		Console.Write("Издательство: ");
		string publisher = Console.ReadLine();

		_db.AddBook(author, title, year, publisher);
		ShowSuccess("Книга успешно добавлена!");
	}

	static void IssueBookMenu()
	{
		try
		{
			int bookId = ReadInt("ID книги для выдачи: ", 1, int.MaxValue);
			_db.IssueBook(bookId);
			ShowSuccess("Книга выдана успешно!");
		}
		catch (Exception ex)
		{
			ShowError(ex.Message);
		}
	}

	static void ReturnBookMenu()
	{
		try
		{
			int bookId = ReadInt("ID книги для возврата: ", 1, int.MaxValue);
			_db.ReturnBook(bookId);
			ShowSuccess("Книга принята!");
		}
		catch (Exception ex)
		{
			ShowError(ex.Message);
		}
	}

	static void ShowNeverIssued()
	{
		Console.WriteLine("\nНикогда не выданные книги:");
		_db.PrintBooks(_db.GetNeverIssuedBooks());
	}

	static void ShowNotReturned()
	{
		Console.WriteLine("\nКниги в выдаче:");
		_db.PrintBooks(_db.GetNotReturnedBooks());
	}

	static int ReadInt(string prompt, int min, int max)
	{
		while (true)
		{
			Console.Write(prompt);
			if (int.TryParse(Console.ReadLine(), out int result) 
				&& result >= min 
				&& result <= max) return result;
			
			ShowError($"Введите число от {min} до {max}");
		}
	}

	static void ShowError(string message)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine($"\nОшибка: {message}");
		Console.ResetColor();
	}

	static void ShowSuccess(string message)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine($"\n{message}");
		Console.ResetColor();
	}
}
