using System.Text;

namespace Task_Manager;

class Program
{
    static TaskManager manager = new TaskManager("tasks.txt");

    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Title = "Менеджер задач";

        manager.Load();

        while (true)
        {
            ShowMenu();
            manager.Save();
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

        foreach (TaskItem task in manager.GetAll())
        {
            Console.WriteLine(task);
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

        if (manager.Add(id, description))
            Console.WriteLine("Задача добавлена.");
        else
            Console.WriteLine("Задача с таким ID уже была, описание заменено.");

        Pause();
    }

    static void DeleteTask()
    {
        if (manager.Count == 0)
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
            if (manager.Remove(id))
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
                if (manager.Remove(id))
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
        if (manager.Count == 0)
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
        if (manager.Count == 0)
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
            manager.Clear();
            Console.WriteLine("\nСписок очищен.");
        }
        else
        {
            Console.WriteLine("\nОтменено.");
        }

        Pause();
    }
}
