using System;
using System.Collections.Generic;
using System.Threading;

public class Participant
{
	public string Name { get; }
	public double CurrentSpeed { get; private set; }
	public double DistanceCovered { get; private set; }
	private Random _random;

	public Participant(string name, double initialSpeed)
	{
		Name = name;
		CurrentSpeed = initialSpeed;
		_random = new Random();
	}

	public void UpdateSpeed()
	{
		double change = (_random.NextDouble() * 0.6 - 0.3);
		CurrentSpeed *= (1 + change);
	}

	public void Run(double timeInterval)
	{
		DistanceCovered += CurrentSpeed * timeInterval;
	}
}

public class Race
{
	public delegate void WinnerHandler(List<Participant> winners);
	public event WinnerHandler OnWin;

	private double _trackLength;
	private List<Participant> _participants;
	private double _timeInterval;

	public Race(double trackLength, double timeInterval)
	{
		_trackLength = trackLength;
		_timeInterval = timeInterval;
		_participants = new List<Participant>();
	}

	public void AddParticipant(Participant participant)
	{
		_participants.Add(participant);
	}

	public void StartRace()
	{
		Console.WriteLine($"\nГонка началась! Длина трека: {_trackLength} м");

		while(true)
		{
			Thread.Sleep(1000);

			foreach(var p in _participants)
			{
				p.UpdateSpeed();
				p.Run(_timeInterval);
			}

			var winners = new List<Participant>();
			foreach(var p in _participants)
			{
				if(p.DistanceCovered >= _trackLength)
				{
					winners.Add(p);
				}
			}

			if(winners.Count > 0)
			{
				OnWin?.Invoke(winners);
				break;
			}
		}
	}
}

class Program
{
	static void Main()
	{
		double trackLength = ReadDouble("Введите длину трека (метров): ", 100, 10000);
		double timeInterval = ReadDouble("Введите интервал времени (секунд): ", 0.1, 10);

		var race = new Race(trackLength, timeInterval);
		AddParticipantsManually(race);

		race.OnWin += winners => 
		{
			Console.WriteLine("\nРезультаты гонки:");
			foreach(var w in winners)
			{
				Console.WriteLine($"{w.Name} - {w.DistanceCovered:F2} м");
			}
		};

		race.StartRace();
	}

	static void AddParticipantsManually(Race race)
	{
		int participantCount;
		while(true)
		{
			Console.Write("Введите количество участников: ");
			if(int.TryParse(Console.ReadLine(), out participantCount) && participantCount > 0) break;
			Console.WriteLine("Некорректный ввод! Введите целое число больше 0.");
		}

		for(int i = 0; i < participantCount; i++)
		{
			Console.WriteLine($"\nУчастник #{i + 1}");
			string name = ReadName();
			double speed = ReadDouble("Начальная скорость (м/с): ", 1, 20);
			race.AddParticipant(new Participant(name, speed));
		}
	}

	static string ReadName()
	{
		string name;
		do
		{
			Console.Write("Имя участника: ");
			name = Console.ReadLine().Trim();
			if(string.IsNullOrEmpty(name)) Console.WriteLine("Имя не может быть пустым!");
		} while(string.IsNullOrEmpty(name));

		return name;
	}

	static double ReadDouble(string prompt, double min, double max)
	{
		double value;
		while(true)
		{
			Console.Write(prompt);
			if(double.TryParse(Console.ReadLine(), out value) 
				&& value >= min 
				&& value <= max) break;

			Console.WriteLine($"Ошибка! Введите число от {min} до {max}");
		}
		return value;
	}
}
