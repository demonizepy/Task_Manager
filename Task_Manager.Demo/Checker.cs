namespace Task_Manager.Demo;

public class Checker
{
    public int Passed { get; private set; }
    public int Failed { get; private set; }

    public void Check(string what, bool ok)
    {
        if (ok)
        {
            Passed++;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("  [OK]     ");
        }
        else
        {
            Failed++;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write("  [ОШИБКА] ");
        }

        Console.ResetColor();
        Console.WriteLine(what);
    }
}
