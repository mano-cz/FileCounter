using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileCounter
{
    internal class Program
    {
        static async Task Main()
        {
            var random = new Random();
            while (true)
            {
                await using var fs = new FileStream(@"counter.dat", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
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
                if (ShouldGenerateCpuSpike(random))
                {
                    Console.WriteLine("CPU spike");
                    await GenerateCpuSpikeFor5Sec();
                }
            }
        }

        static bool ShouldGenerateCpuSpike(Random random)
        {
            return random.Next(1, 10) == 8;
        }

        static async Task GenerateCpuSpikeFor5Sec()
        {
            var cts = new CancellationTokenSource();
            var cancelToken = cts.Token;
            const int taskCount = 1000;
            var tasks = new List<Task>(1000);
            for (var i = 0; i < taskCount; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    while (true)
                    {
                        if (cancelToken.IsCancellationRequested)
                            break;
                    }
                }, cancelToken));
            }
            tasks.Add(Task.Delay(TimeSpan.FromSeconds(5)));
            await Task.WhenAny(tasks);
            cts.Cancel();
        }
    }
}
