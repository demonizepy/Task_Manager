using System.Text;
using Task_Manager.Core.Models;
using Task_Manager.Core.Services;

namespace Task_Manager.Demo;

class Program
{
    static readonly Checker checker = new();

    // отдельный файл, чтобы не трогать задачи из основной программы
    static readonly string FilePath = Path.Combine(Path.GetTempPath(), "task_manager_demo.txt");

    static int Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.WriteLine("Демонстрация модуля Task_Manager.Core");
        Console.WriteLine("Файл: " + FilePath);

        File.Delete(FilePath);

        AddAndSave();
        LoadInNewManager();
        ReplaceTask();
        RemoveByIds();
        OldFileFormat();
        ClearAll();
        LineBreakInDescription();

        Console.WriteLine();
        Console.WriteLine($"Итог: пройдено {checker.Passed}, ошибок {checker.Failed}");

        File.Delete(FilePath);
        return checker.Failed;
    }

    static TaskManager CreateManager()
    {
        return new TaskManager(new TaskStorage(FilePath));
    }

    static void Title(string text)
    {
        Console.WriteLine();
        Console.WriteLine(text);
    }

    static void AddAndSave()
    {
        Title("1. Добавление задач и сохранение в файл");

        TaskManager manager = CreateManager();
        manager.Add(1, "Сделать лабораторную");
        manager.Add(2, "Купить хлеб | молоко");
        manager.Add(5, "Подготовить отчёт");
        manager.Save();

        string[] lines = File.ReadAllLines(FilePath);
        Console.WriteLine("  Содержимое файла:");
        foreach (string line in lines)
            Console.WriteLine("    " + line);

        checker.Check("в менеджере 3 задачи", manager.Count == 3);
        checker.Check("в файле 3 строки", lines.Length == 3);
        checker.Check("следующий свободный ID равен 6", manager.GetNextId() == 6);
    }

    static void LoadInNewManager()
    {
        Title("2. Загрузка тех же задач новым экземпляром менеджера");

        TaskManager manager = CreateManager();
        manager.Load();

        foreach (TaskItem task in manager.GetAll())
            Console.WriteLine($"  ID: {task.Id}, описание: {task.Description}");

        List<TaskItem> tasks = manager.GetAll();
        checker.Check("загружено 3 задачи", tasks.Count == 3);
        checker.Check("символ | в описании не потерялся", tasks[1].Description == "Купить хлеб | молоко");
        checker.Check("задачи идут по возрастанию ID", tasks.Select(t => t.Id).SequenceEqual(new[] { 1, 2, 5 }));
    }

    static void ReplaceTask()
    {
        Title("3. Добавление задачи с уже существующим ID");

        TaskManager manager = CreateManager();
        manager.Load();

        bool isNew = manager.Add(1, "Сдать лабораторную");
        manager.Save();

        TaskManager check = CreateManager();
        check.Load();

        checker.Check("Add вернул false, задача заменена", !isNew);
        checker.Check("количество не изменилось", check.Count == 3);
        checker.Check("в файле новое описание", check.GetAll()[0].Description == "Сдать лабораторную");
    }

    static void RemoveByIds()
    {
        Title("4. Удаление по списку ID: \"2, 7, abc\"");

        TaskManager manager = CreateManager();
        manager.Load();

        RemoveResult result = manager.RemoveByIds("2, 7, abc");
        manager.Save();

        Console.WriteLine("  Удалено: " + string.Join(", ", result.Removed));
        Console.WriteLine("  Не найдено: " + string.Join(", ", result.NotFound));
        Console.WriteLine("  Неверный ID: " + string.Join(", ", result.Invalid));

        checker.Check("удалена задача 2", result.Removed.SequenceEqual(new[] { 2 }));
        checker.Check("задача 7 не найдена", result.NotFound.SequenceEqual(new[] { 7 }));
        checker.Check("abc распознан как неверный ID", result.Invalid.SequenceEqual(new[] { "abc" }));
        checker.Check("в файле осталось 2 строки", File.ReadAllLines(FilePath).Length == 2);
    }

    static void OldFileFormat()
    {
        Title("5. Чтение файла старого формата и строк с мусором");

        File.WriteAllLines(FilePath, new[] { "10| старая задача", "мусорная строка", "11|ещё одна" });

        TaskManager manager = CreateManager();
        manager.Load();

        checker.Check("мусорная строка пропущена", manager.Count == 2);
        checker.Check("лишний пробел после | обрезан", manager.GetAll()[0].Description == "старая задача");
    }

    static void ClearAll()
    {
        Title("6. Очистка списка");

        TaskManager manager = CreateManager();
        manager.Load();
        manager.Clear();
        manager.Save();

        TaskManager check = CreateManager();
        check.Load();

        checker.Check("после очистки задач нет", check.Count == 0);
        checker.Check("для пустого списка следующий ID равен 1", check.GetNextId() == 1);
    }

    static void LineBreakInDescription()
    {
        Title("7. Перенос строки внутри описания");

        TaskManager manager = CreateManager();
        manager.Add(20, "первая строка\nвторая строка");
        manager.Save();

        checker.Check("перенос заменён пробелом", manager.GetAll()[0].Description == "первая строка вторая строка");
        checker.Check("в файле одна строка", File.ReadAllLines(FilePath).Length == 1);
    }
}
