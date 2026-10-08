namespace Task_Manager.UI;

public static class ConsoleInput
{
    public static int ReadInt(string prompt)
    {
        Console.Write(prompt);

        while (true)
        {
            if (int.TryParse(Console.ReadLine(), out int value))
                return value;

            Console.Write("Нужно ввести число: ");
        }
    }

    public static string ReadText(string prompt)
    {
        Console.Write(prompt);
        return (Console.ReadLine() ?? "").Trim();
    }

    public static void Pause()
    {
        Console.WriteLine("\nНажмите любую клавишу, чтобы продолжить...");
        Console.ReadKey();
        Console.Clear();
    }
}
