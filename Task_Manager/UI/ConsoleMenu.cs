using Task_Manager.Models;
using Task_Manager.Services;

namespace Task_Manager.UI;

public class ConsoleMenu
{
    private readonly TaskManager manager;

    public ConsoleMenu(TaskManager manager)
    {
        this.manager = manager;
    }

    public void Run()
    {
        bool running = true;

        while (running)
        {
            ShowMenu();

            switch (ConsoleInput.ReadInt("Выберите пункт: "))
            {
                case 1:
                    AddTask();
                    break;
                case 2:
                    DeleteTasks();
                    break;
                case 3:
                    ShowTasks();
                    break;
                case 4:
                    ClearTasks();
                    break;
                case 5:
                    running = !ConfirmExit();
                    break;
                default:
                    Console.WriteLine("\nТакого пункта нет, попробуйте ещё раз.");
                    break;
            }

            manager.Save();
        }
    }

    private void ShowMenu()
    {
        Console.WriteLine("\n1. Добавить задачу");
        Console.WriteLine("2. Удалить задачу");
        Console.WriteLine("3. Показать задачи");
        Console.WriteLine("4. Очистить список");
        Console.WriteLine("5. Выход");
    }

    private void PrintTasks()
    {
        Console.WriteLine("\nЗадачи:");

        foreach (TaskItem task in manager.GetAll())
        {
            Console.WriteLine(task);
        }
    }

    private bool ConfirmExit()
    {
        Console.WriteLine("\nПродолжить или выйти?");
        Console.WriteLine("1. Продолжить");
        Console.WriteLine("2. Выйти");

        while (true)
        {
            int answer = ConsoleInput.ReadInt("Выберите пункт: ");

            if (answer == 1)
            {
                Console.Clear();
                return false;
            }

            if (answer == 2)
                return true;

            Console.WriteLine("Такого пункта нет.");
        }
    }

    private bool HasTasks()
    {
        if (manager.Count > 0)
            return true;

        Console.WriteLine("\nЗадач пока нет.");
        ConsoleInput.Pause();
        return false;
    }

    private void AddTask()
    {
        int id = ConsoleInput.ReadInt("\nВведите ID задачи: ");
        string description = ConsoleInput.ReadText("Введите описание задачи: ");

        if (description == "")
        {
            Console.WriteLine("Описание не может быть пустым.");
        }
        else if (manager.Add(id, description))
        {
            Console.WriteLine("Задача добавлена.");
        }
        else
        {
            Console.WriteLine("Задача с таким ID уже была, описание заменено.");
        }

        ConsoleInput.Pause();
    }

    private void DeleteTasks()
    {
        if (!HasTasks())
            return;

        PrintTasks();

        string input = ConsoleInput.ReadText("\nВведите ID задачи (или несколько через запятую): ");
        RemoveResult result = manager.RemoveByIds(input);

        if (result.IsEmpty)
            Console.WriteLine("Ничего не введено.");

        if (result.Removed.Count > 0)
            Console.WriteLine("Удалено: " + string.Join(", ", result.Removed));

        if (result.NotFound.Count > 0)
            Console.WriteLine("Не найдено: " + string.Join(", ", result.NotFound));

        if (result.Invalid.Count > 0)
            Console.WriteLine("Неверный ID: " + string.Join(", ", result.Invalid));

        ConsoleInput.Pause();
    }

    private void ShowTasks()
    {
        if (!HasTasks())
            return;

        PrintTasks();
        ConsoleInput.Pause();
    }

    private void ClearTasks()
    {
        if (!HasTasks())
            return;

        PrintTasks();

        Console.WriteLine("\nТочно очистить весь список?");
        Console.WriteLine("1. Да");
        Console.WriteLine("2. Нет");

        if (ConsoleInput.ReadInt("Выберите пункт: ") == 1)
        {
            manager.Clear();
            Console.WriteLine("\nСписок очищен.");
        }
        else
        {
            Console.WriteLine("\nОтменено.");
        }

        ConsoleInput.Pause();
    }
}
