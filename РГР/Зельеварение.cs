using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static string ProcessAction(string[] parts)
    {
        string action = parts[0];
        string ingredients = "";

        // Собираем ингредиенты
        for (int i = 1; i < parts.Length; i++)
        {
            if (int.TryParse(parts[i], out int actionNumber))
            {
                // Если это ссылка на предыдущее действие
                ingredients += actions[actionNumber - 1];
            }
            else
            {
                // Если это название ингредиента
                ingredients += parts[i];
            }
        }

        // Формируем слово в зависимости от действия
        switch (action)
        {
            case "MIX":
                return "MX" + ingredients + "XM";
            case "WATER":
                return "WT" + ingredients + "TW";
            case "DUST":
                return "DT" + ingredients + "TD";
            case "FIRE":
                return "FR" + ingredients + "RF";
            default:
                return "";
        }
    }

    static List<string> actions = new List<string>();

    static void Main()
    {
        string[] lines = File.ReadAllLines("input.txt");

        // Обрабатываем каждое действие
        foreach (string line in lines)
        {
            string[] parts = line.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            string result = ProcessAction(parts);
            actions.Add(result);
        }

        // Записываем последнее действие как результат
        File.WriteAllText("output.txt", actions[actions.Count - 1]);
    }
}