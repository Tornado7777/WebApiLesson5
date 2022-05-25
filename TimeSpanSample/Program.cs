using System;
using System.Threading;

namespace TimeSpanSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            TimeSpan timeStart = TimeSpan.FromMilliseconds(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            Random random = new Random();
            Thread.Sleep(random.Next(2500, 3000));

            TimeSpan timeEnd = TimeSpan.FromMilliseconds(DateTimeOffset.UtcNow.ToUnixTimeMilliseconds());

            TimeSpan timeDiff = timeEnd - timeStart;

            Console.WriteLine(timeDiff.ToString("dd\\.hh\\:mm\\:ss\\:FFF"));

            Console.ReadKey(true);
        }
    }
}
