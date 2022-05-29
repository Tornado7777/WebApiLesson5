using System;

using System.Threading;

using System.Diagnostics;
//using System.Diagnostics.PerformanceData;

using System.Threading.Tasks;
using System.Text;
using System.Collections.Generic;

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

            // Console.ReadKey(true);
            // Пример работы с PerformanceCounter
            var _cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            float cpuUsageInPercents = _cpuCounter.NextValue();
            Console.WriteLine(cpuUsageInPercents.ToString());
            _cpuCounter.Dispose();

            var _hddCounter = new PerformanceCounter("PhysicalDisk", "Disk Reads/sec", "_Total");
            float hddUsageInPercents = _hddCounter.NextValue();
            Console.WriteLine(hddUsageInPercents.ToString());
            _hddCounter.Dispose();

            var _ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            float ramUsageInPercents = _ramCounter.NextValue();
            Console.WriteLine(ramUsageInPercents.ToString());
            _ramCounter.Dispose();

            //var cat = new PerformanceCounterCategory(".NET CLR Memory");
            //cat.GetInstanceNames();

            //var _dotNetCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all Heaps", Process.GetCurrentProcess().ProcessName, true);
            var _dotNetCounter = new PerformanceCounter(".NET CLR Memory", "# Bytes in all Heaps", "_Global_");
            //var _dotNetCounter = new PerformanceCounter(".NET CLR Exceptions", "# of Exceps Thrown / sec", "_Global_");
            float dotNetUsageInPercents = _dotNetCounter.NextValue();
            Console.WriteLine(dotNetUsageInPercents.ToString());
            _dotNetCounter.Dispose();

            Console.WriteLine(GC.GetTotalAllocatedBytes());


            PerformanceCounterCategory _categoryNetwork = new PerformanceCounterCategory("Network Interface");
            String[] nameNetwork = _categoryNetwork.GetInstanceNames();
            float networkUsageRecivedInSec= 0;

            foreach (string name in nameNetwork)
            {
                var _networkCounter = new PerformanceCounter("Network Interface", "Bytes Received/sec", name );
                networkUsageRecivedInSec += _networkCounter.NextValue();
                
            }
            networkUsageRecivedInSec /= nameNetwork.Length;
            Console.WriteLine(networkUsageRecivedInSec.ToString());

            //PrintPerformanceCounterParameters();
            Console.ReadLine();
        }

        private static void PrintPerformanceCounterParameters()
        {
            var sb = new StringBuilder();
            PerformanceCounterCategory[] categories = PerformanceCounterCategory.GetCategories();

            var desiredCategories = new HashSet<string> { "LogicalDisk" };

            foreach (var category in categories)
            {
                sb.AppendLine("Category: " + category.CategoryName);
                if (desiredCategories.Contains(category.CategoryName))
                {
                    PerformanceCounter[] counters;
                    try
                    {
                        counters = category.GetCounters("devenv");
                    }
                    catch (Exception)
                    {
                        counters = category.GetCounters();
                    }

                    foreach (var counter in counters)
                    {
                        sb.AppendLine(counter.CounterName + ": " + counter.CounterHelp);
                    }
                }
            }
            Console.WriteLine(sb.ToString());
            //File.WriteAllText(@"C:\performanceCounters.txt", sb.ToString());
        }
    }
}
