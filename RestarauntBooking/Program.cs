using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace RestarauntBooking
{
    internal static class Program
    {
        private static async Task Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            var rest = new Restaurant();
            while (true)
            {
                await Task.Delay(10000);

                Console.WriteLine("Привет! Желаете забронировать столик?");

                var stopWatch = new Stopwatch();
                stopWatch.Start();
                
                rest.BookFreeTableAsync(1);

                Console.WriteLine("Спасибо за Ваше обращение!");
                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
            }
        }
    }
}