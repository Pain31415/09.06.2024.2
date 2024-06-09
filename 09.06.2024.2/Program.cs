using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static Mutex mutex = new Mutex();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.InputEncoding = System.Text.Encoding.Unicode;

        Console.WriteLine("Натисніть будь-яку клавішу для запуску потоків...");
        Console.ReadKey();

        Task firstTask = Task.Run(() => FirstThread());
        firstTask.ContinueWith(t => SecondThread());

        Console.WriteLine("Потоки запущені. Натисніть будь-яку клавішу для завершення програми...");
        Console.ReadKey();
    }

    static void FirstThread()
    {
        mutex.WaitOne();
        Console.ForegroundColor = ConsoleColor.Green;
        for (int i = 0; i <= 20; i++)
        {
            Console.WriteLine($"Перший потік: {i}");
            Thread.Sleep(100);
        }
        Console.ResetColor();
        mutex.ReleaseMutex();
    }

    static void SecondThread()
    {
        mutex.WaitOne();
        Console.ForegroundColor = ConsoleColor.Cyan;
        for (int i = 10; i >= 0; i--)
        {
            Console.WriteLine($"Другий потік: {i}");
            Thread.Sleep(100);
        }
        Console.ResetColor();
        mutex.ReleaseMutex();
    }
}
