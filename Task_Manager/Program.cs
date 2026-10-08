using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

class Program
{
    const string FileName = "tasks.txt";

    static Dictionary<int, string> tasks = new Dictionary<int, string>();

    static void Main()
    {
        // без этого русские буквы в консоли могут превратиться в вопросики
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Title = "Менеджер задач";

        LoadTasks();

        while (true)
        {
            ShowMenu();
            SaveTasks();
        }
    }

    static void ShowMenu()
    {
        Console.WriteLine("\n1. Добавить задачу");
        Console.WriteLine("2. Удалить задачу");
        Console.WriteLine("3. Показать задачи");
        Console.WriteLine("4. Очистить список");
        Console.WriteLine("5. Выход");
        Console.Write("Выберите пункт: ");

        int choice = ReadInt();

        switch (choice)
        {
            case 1:
                AddTask();
                break;
            case 2:
                DeleteTask();
                break;
            case 3:
                DisplayTasks();
                break;
            case 4:
                ClearAllTasks();
                break;
            case 5:
                ContinueExit();
                break;
            default:
                Console.WriteLine("\nТакого пункта нет, попробуйте ещё раз.");
                break;
        }
    }

    static int ReadInt()
    {
        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int value))
                return value;

            Console.Write("Нужно ввести число: ");
        }
    }

    static void Pause()
    {
        Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
        Console.ReadKey();
        Console.Clear();
    }

    static void PrintTasks()
    {
        Console.WriteLine("\nЗадачи:");

        foreach (var task in tasks)
        {
            Console.WriteLine($"ID: {task.Key}, описание: {task.Value}");
        }
    }

    static void ContinueExit()
    {
        Console.WriteLine("\nПродолжить или выйти?");
        Console.WriteLine("1. Продолжить");
        Console.WriteLine("2. Выйти");
        Console.Write("Выберите пункт: ");

        int option = ReadInt();

        switch (option)
        {
            case 1:
                Console.Clear();
                break;
            case 2:
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("\nТакого пункта нет, попробуйте ещё раз.");
                ContinueExit();
                break;
        }
    }

    static void AddTask()
    {
        Console.Write("\nВведите ID задачи: ");
        int id = ReadInt();

        Console.Write("Введите описание задачи: ");
        string description = (Console.ReadLine() ?? "").Trim();

        if (description == "")
        {
            Console.WriteLine("Описание не может быть пустым.");
            Pause();
            return;
        }

        tasks[id] = description;
        Console.WriteLine("Задача добавлена.");

        Pause();
    }

    static void DeleteTask()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("\nЗадач пока нет.");
            Pause();
            return;
        }

        PrintTasks();

        Console.Write("\nВведите ID задачи (или несколько через запятую): ");
        string input = Console.ReadLine() ?? "";

        if (input.Contains(","))
        {
            DeleteTask(input);
            return;
        }

        if (int.TryParse(input, out int id))
        {
            if (tasks.Remove(id))
                Console.WriteLine("Задача удалена.");
            else
                Console.WriteLine("Задача не найдена.");
        }
        else
        {
            Console.WriteLine("Неверный ID.");
        }

        Pause();
    }

    static void DeleteTask(string ids)
    {
        string[] parts = ids.Split(',');

        foreach (string part in parts)
        {
            if (int.TryParse(part.Trim(), out int id))
            {
                if (tasks.Remove(id))
                    Console.WriteLine($"Задача {id} удалена.");
                else
                    Console.WriteLine($"Задача {id} не найдена.");
            }
            else
            {
                Console.WriteLine($"Неверный ID: {part.Trim()}");
            }
        }

        Pause();
    }

    static void DisplayTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("\nЗадач пока нет.");
            Pause();
            return;
        }

        PrintTasks();
        Pause();
    }

    static void ClearAllTasks()
    {
        if (tasks.Count == 0)
        {
            Console.WriteLine("\nЗадач пока нет.");
            Pause();
            return;
        }

        PrintTasks();

        Console.WriteLine("\nТочно очистить весь список?");
        Console.WriteLine("1. Да");
        Console.WriteLine("2. Нет");
        Console.Write("Выберите пункт: ");

        if (ReadInt() == 1)
        {
            tasks.Clear();
            Console.WriteLine("\nСписок очищен.");
        }
        else
        {
            Console.WriteLine("\nОтменено.");
        }

        Pause();
    }

    static void SaveTasks()
    {
        using (StreamWriter writer = new StreamWriter(FileName))
        {
            foreach (var task in tasks)
            {
                writer.WriteLine($"{task.Key}|{task.Value}");
            }
        }
    }

    static void LoadTasks()
    {
        if (!File.Exists(FileName))
            return;

        foreach (var line in File.ReadAllLines(FileName))
        {
            // раньше после | писался пробел, его надо обрезать
            string[] parts = line.Split('|', 2);

            if (parts.Length < 2 || !int.TryParse(parts[0], out int id))
                continue;

            tasks[id] = parts[1].Trim();
        }
    }
}
