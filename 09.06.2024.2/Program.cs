using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

class Program
{
    static Mutex mutex = new Mutex();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.Unicode;
        Console.InputEncoding = System.Text.Encoding.Unicode;

        Thread firstThread = new Thread(GenerateRandomNumbers);
        Thread secondThread = new Thread(FindPrimeNumbers);
        Thread thirdThread = new Thread(FindPrimeNumbersEndingWithSeven);
        Thread fourthThread = new Thread(GenerateReport);

        firstThread.Start();
        secondThread.Start();
        thirdThread.Start();
        fourthThread.Start();
    }

    static void GenerateRandomNumbers()
    {
        mutex.WaitOne();

        try
        {
            List<int> numbers = new List<int>();
            Random rand = new Random();

            for (int i = 0; i < 100; i++)
            {
                numbers.Add(rand.Next(1, 1000));
            }

            File.WriteAllLines("random_numbers.txt", numbers.Select(n => n.ToString()));
            Console.WriteLine("Перший потік: Записано випадкові числа у файл.");
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    static void FindPrimeNumbers()
    {
        mutex.WaitOne();

        try
        {
            string[] lines = File.ReadAllLines("random_numbers.txt");
            List<int> primes = new List<int>();

            foreach (string line in lines)
            {
                int num = int.Parse(line);
                if (IsPrime(num))
                    primes.Add(num);
            }

            File.WriteAllLines("prime_numbers.txt", primes.Select(p => p.ToString()));
            Console.WriteLine("Другий потік: Записано прості числа у файл.");
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    static void FindPrimeNumbersEndingWithSeven()
    {
        mutex.WaitOne();

        try
        {
            string[] lines = File.ReadAllLines("prime_numbers.txt");
            List<int> primesEndingWithSeven = new List<int>();

            foreach (string line in lines)
            {
                int num = int.Parse(line);
                if (num % 10 == 7)
                    primesEndingWithSeven.Add(num);
            }

            File.WriteAllLines("prime_numbers_ending_with_seven.txt", primesEndingWithSeven.Select(p => p.ToString()));
            Console.WriteLine("Третій потік: Записано прості числа, які закінчуються на 7, у файл.");
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    static void GenerateReport()
    {
        mutex.WaitOne();

        try
        {
            using (StreamWriter writer = new StreamWriter("report.txt"))
            {
                string[] files = { "random_numbers.txt", "prime_numbers.txt", "prime_numbers_ending_with_seven.txt" };

                foreach (string file in files)
                {
                    string[] lines = File.ReadAllLines(file);

                    writer.WriteLine($"Звіт для файлу {file}:");
                    writer.WriteLine($"Кількість чисел: {lines.Length}");
                    writer.WriteLine($"Розмір файлу (байт): {new FileInfo(file).Length}");

                    writer.WriteLine("Вміст файлу:");
                    foreach (string line in lines)
                    {
                        writer.WriteLine(line);
                    }

                    writer.WriteLine();
                }
            }

            Console.WriteLine("Четвертий потік: Записано звіт у файл.");
        }
        finally
        {
            mutex.ReleaseMutex();
        }
    }

    static bool IsPrime(int num)
    {
        if (num <= 1) return false;
        if (num == 2) return true;
        if (num % 2 == 0) return false;

        for (int i = 3; i * i <= num; i += 2)
        {
            if (num % i == 0)
                return false;
        }

        return true;
    }
}
