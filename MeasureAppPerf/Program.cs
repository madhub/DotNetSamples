using System;
using System.Diagnostics;

namespace MeasureAppPerf
{
    class Program
    {
        static void Main(string[] args)
        {
            AppDomain.MonitoringIsEnabled = true;
            // do some activity here: https://medium.com/@indy_singh/strings-are-evil-a803d05e5ce3

            Console.WriteLine($"Took: {AppDomain.CurrentDomain.MonitoringTotalProcessorTime.TotalMilliseconds:#,###} ms");

            Console.WriteLine($"Allocated: {AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize / 1024:#,#} kb");

            Console.WriteLine($"Peak Working Set: {Process.GetCurrentProcess().PeakWorkingSet64 / 1024:#,#} kb");



            for (int index = 0; index <= GC.MaxGeneration; index++)
            {
                Console.WriteLine($"Gen {index} collections: {GC.CollectionCount(index)}");
            }
        }
    }
}
