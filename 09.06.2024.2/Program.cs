using System;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static SemaphoreSlim semaphore = new SemaphoreSlim(3);
    static Random random = new Random();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.InputEncoding = System.Text.Encoding.Unicode;

        Task[] tasks = new Task[10];

        for (int i = 0; i < 10; i++)
        {
            int threadIndex = i;
            tasks[threadIndex] = Task.Run(() => ThreadWork(threadIndex));
        }

        Task.WaitAll(tasks);

        Console.WriteLine("Усі потоки завершили роботу.");
        Console.WriteLine("Натисніть будь-яку клавішу для завершення програми...");
        Console.ReadKey();
    }

    static void ThreadWork(int threadIndex)
    {
        semaphore.Wait();

        try
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Потік {threadIndex} починає роботу");

            Console.ForegroundColor = ConsoleColor.Yellow;
            for (int i = 0; i < 5; i++)
            {
                Console.WriteLine($"Потік {threadIndex}: {random.Next(1, 100)}");
                Thread.Sleep(100);
            }
        }
        finally
        {
            Console.ResetColor();
            Console.WriteLine($"Потік {threadIndex} завершує роботу");
            semaphore.Release();
        }
    }
}
