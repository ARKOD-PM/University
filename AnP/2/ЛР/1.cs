/* Implementation of work with the telephone directory database.
 * The objects are characterized by the following values: full name, phone number (one person may have several numbers), telecom operator (assigned to the phone), date of conclusion of the contract, city.
 * A selection has been implemented by the date of conclusion of the contract, by telecom operator, by phone number, by city.
 */

using System;
using System.Collections.Generic;
using System.Linq;

public class Phone
{
	public string Number { get; set; }
	public string Operator { get; set; }
	public DateTime ContractDate { get; set; }
}

public class Person
{
	public string FullName { get; set; }
	public string City { get; set; }
	public List<Phone> Phones { get; set; } = new List<Phone>();
}

public class PhoneBookService
{
	private List<Person> _persons = new List<Person>();

	public void AddPerson(Person person) => _persons.Add(person);

	public void AddPhoneToPerson(Person person, Phone phone) => 
	person.Phones.Add(phone);

	public List<Person> SearchByDate(DateTime date) => 
	_persons.Where(p => 
	p.Phones.Any(ph => ph.ContractDate.Date == date.Date)
	).ToList();

	public List<Person> SearchByOperator(string operatorName) => 
	_persons.Where(p => 
	p.Phones.Any(ph => ph.Operator == operatorName)
	).ToList();

	public Person SearchByPhoneNumber(string number) => 
	_persons.FirstOrDefault(p => 
	p.Phones.Any(ph => ph.Number == number)
	);

	public List<Person> SearchByCity(string city) => 
	_persons.Where(p => p.City == city).ToList();

	public List<Person> GetAllPeople() => _persons;
}

class Program
{
	static PhoneBookService service = new PhoneBookService();

	static void Main()
	{
		while (true)
		{
			Console.Clear();
			Console.WriteLine("Telephone Directory");
			Console.WriteLine("1. Show all entries");
			Console.WriteLine("2. Add a new entry");
			Console.WriteLine("3. Search by contract date");
			Console.WriteLine("4. Search by operator");
			Console.WriteLine("5. Search by phone number");
			Console.WriteLine("6. Search by city");
			Console.WriteLine("7. Exit");
			Console.Write("Select action: ");

			switch (Console.ReadLine())
			{
				case "1": ShowAll(); break;
				case "2": AddNewRecord(); break;
				case "3": SearchByDateMenu(); break;
				case "4": SearchByOperatorMenu(); break;
				case "5": SearchByPhoneMenu(); break;
				case "6": SearchByCityMenu(); break;
				case "7": return;
				default: Console.WriteLine("Wrong input."); break;
			}
			Console.WriteLine("\nPress any key...");
			Console.ReadKey();
		}
	}

	static void ShowAll()
	{
		var people = service.GetAllPeople();
		PrintPeople(people);
	}

	static void AddNewRecord()
	{
		var person = new Person();

		Console.Write("Full name: ");
		person.FullName = Console.ReadLine();

		Console.Write("City: ");
		person.City = Console.ReadLine();

		service.AddPerson(person);

		Console.Write("Add phone? (y/n): ");
		if (Console.ReadLine().ToLower() == "y")
		{
			var phone = new Phone();

			Console.Write("Phone number: ");
			phone.Number = Console.ReadLine();

			Console.Write("Operator: ");
			phone.Operator = Console.ReadLine();

			Console.Write("Contract date (yyyy-mm-dd): ");
			phone.ContractDate = DateTime.Parse(Console.ReadLine());

			service.AddPhoneToPerson(person, phone);
		}
	}

	static void SearchByDateMenu()
	{
		Console.Write("Enter date (yyyy-mm-dd): ");
		var date = DateTime.Parse(Console.ReadLine());
		var results = service.SearchByDate(date);
		PrintPeople(results);
	}

	static void SearchByOperatorMenu()
	{
		Console.Write("Enter operator: ");
		var operatorName = Console.ReadLine();
		var results = service.SearchByOperator(operatorName);
		PrintPeople(results);
	}

	static void SearchByPhoneMenu()
	{
		Console.Write("Enter phone number: ");
		var number = Console.ReadLine();
		var person = service.SearchByPhoneNumber(number);
		PrintPeople(person != null ? new List<Person> { person } : new List<Person>());
	}

	static void SearchByCityMenu()
	{
		Console.Write("Enter city: ");
		var city = Console.ReadLine();
		var results = service.SearchByCity(city);
		PrintPeople(results);
	}

	static void PrintPeople(IEnumerable<Person> people)
	{
		foreach (var person in people)
		{
			Console.WriteLine($"\nName: {person.FullName}");
			Console.WriteLine($"City: {person.City}");
			Console.WriteLine("Phones:");
			foreach (var phone in person.Phones)
			{
				Console.WriteLine($"  {phone.Number} ({phone.Operator})");
				Console.WriteLine($"  Contract Date: {phone.ContractDate:yyyy-MM-dd}");
			}
			Console.WriteLine(new string('-', 30));
		}
	}
}
