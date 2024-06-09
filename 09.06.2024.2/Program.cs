using System;
using System.Threading;

class Program
{
    private const string MutexName = "Global\\MyUniqueAppMutex";

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.InputEncoding = System.Text.Encoding.Unicode;
        bool isMutexCreated;
        using (Mutex mutex = new Mutex(true, MutexName, out isMutexCreated))
        {
            if (!isMutexCreated)
            {
                ShowAlreadyRunningMessage();
                return;
            }

            try
            {
                RunApplication();
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }
    }

    private static void ShowAlreadyRunningMessage()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Попередження: Програма вже запущена. Запуск другої копії неможливий.");
        Console.ResetColor();
        WaitForUserInput();
    }

    private static void RunApplication()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("Програма успішно запущена. Натисніть будь-яку клавішу для виходу...");
        Console.ResetColor();
        WaitForUserInput();
    }

    private static void WaitForUserInput()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("Натисніть будь-яку клавішу для виходу...");
        Console.ResetColor();
        Console.ReadKey();
    }
}
