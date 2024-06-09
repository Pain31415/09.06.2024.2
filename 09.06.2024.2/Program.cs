using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static Mutex mutex = new Mutex();
    static int[] array = new int[10];
    static Random random = new Random();
    static int maxValue;

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.InputEncoding = System.Text.Encoding.Unicode;

        InitializeArray();

        Console.WriteLine("Натисніть будь-яку клавішу для запуску потоків...");
        Console.ReadKey();

        Task firstTask = Task.Run(() => FirstThread());
        firstTask.ContinueWith(t => SecondThread()).Wait();

        DisplayResults();

        Console.WriteLine("Натисніть будь-яку клавішу для завершення програми...");
        Console.ReadKey();
    }

    static void InitializeArray()
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = random.Next(1, 100);
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Початковий масив: " + string.Join(", ", array));
        Console.ResetColor();
    }

    static void FirstThread()
    {
        mutex.WaitOne();
        Console.ForegroundColor = ConsoleColor.Green;
        for (int i = 0; i < array.Length; i++)
        {
            array[i] += random.Next(1, 10);
            Thread.Sleep(100);
        }
        Console.WriteLine("Модифікований масив (перший потік): " + string.Join(", ", array));
        Console.ResetColor();
        mutex.ReleaseMutex();
    }

    static void SecondThread()
    {
        mutex.WaitOne();
        maxValue = array.Max();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Максимальне значення (другий потік): " + maxValue);
        Console.ResetColor();
        mutex.ReleaseMutex();
    }

    static void DisplayResults()
    {
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("Модифікований масив (поза потоками): " + string.Join(", ", array));
        Console.WriteLine("Максимальне значення (поза потоками): " + maxValue);
        Console.ResetColor();
    }
}
