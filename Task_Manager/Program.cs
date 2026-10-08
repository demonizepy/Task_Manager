using System.Text;
using Task_Manager.Services;
using Task_Manager.UI;

namespace Task_Manager;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = Encoding.UTF8;
        Console.InputEncoding = Encoding.UTF8;
        Console.Title = "Менеджер задач";

        string path = Path.Combine(AppContext.BaseDirectory, "tasks.txt");
        var manager = new TaskManager(new TaskStorage(path));
        manager.Load();

        new ConsoleMenu(manager).Run();
    }
}
