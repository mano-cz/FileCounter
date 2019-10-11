using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileCounter
{
    internal class Program
    {
        static async Task Main()
        {
            while (true)
            {
                await using var fs = new FileStream(@"/app-data/counter.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                using var sr = new StreamReader(fs);
                await using var sw = new StreamWriter(fs);
                var counterValue = await sr.ReadToEndAsync();
                if (!ulong.TryParse(counterValue, out var number))
                {
                    Console.WriteLine("Initialization of new counter.");
                    number = 0;
                }
                fs.Seek(0, SeekOrigin.Begin);
                await sw.WriteLineAsync((++number).ToString());
                Console.WriteLine(number);
                Thread.Sleep(TimeSpan.FromSeconds(1));
            }
        }
    }
}
