using System;
using System.Threading;

class Program
{
    static Mutex mutex = new Mutex(false, "ThreeInstancesMutex");

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.InputEncoding = System.Text.Encoding.Unicode;

        bool isNewInstance;
        mutex = new Mutex(true, "ThreeInstancesMutex", out isNewInstance);

        if (!isNewInstance)
        {
            Console.WriteLine("Додаток може бути запущений тільки в трьох копіях. Третя копія закривається.");
            return;
        }

        Console.WriteLine("Додаток запущено.");
        Console.WriteLine("Натисніть будь-яку клавішу для завершення програми...");
        Console.ReadKey();

        mutex.ReleaseMutex();
    }
}
