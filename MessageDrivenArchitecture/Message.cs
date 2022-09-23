using System;
using System.Diagnostics;

namespace MessageDrivenArchitecture
{
    public class Message
    {
        public void Notification()
        {
            var rest = new Restaurant();
            while (true)
            {
                Console.WriteLine("Привет! Желаете забронировать столик?\n1 - мы уведомим Вас по смс" + 
                                  "\n2 - подождите на линии, мы Вас оповестим" +
                                  "\n3 - снять бронь по смс" +
                                  "\n4 - снять бронь по телефону"); //асинх и синхр
                if (!int.TryParse(Console.ReadLine(), out var choice) && choice is not (1 or 2 or 3 or 4))
                {
                    Console.WriteLine("Введите, пожалуйста 1 или 2 для бронирования или 3, 4 для отмены брони");
                    continue;
                }

                var stopWatch = new Stopwatch();
                stopWatch.Start(); //замер потраченого времени на бронирование.
                
                if (choice == 1)
                {
                    Console.WriteLine("Количество человек?");
                    int ppl = int.Parse(Console.ReadLine());
                    rest.BookFreeTableAsync(ppl); //забронируем ответом по смс
                }
                else if (choice == 2)
                {
                    Console.WriteLine("Количество человек?");
                    int ppl = int.Parse(Console.ReadLine());
                    rest.BookFreeTable(1); // бронь звонком
                }
                else if (choice == 3)
                {
                    Console.WriteLine("Ваш номер забронированного столика?");
                    int table = int.Parse(Console.ReadLine());
                    rest.CancelReservationAsync(table);
                }
                else
                {
                    Console.WriteLine("Ваш номер забронированного столика?");
                    int table = int.Parse(Console.ReadLine());
                    rest.CancelReservation(table);
                }
                
                Console.WriteLine("спасибо за Ваше обращение!");
                stopWatch.Stop();
                var ts = stopWatch.Elapsed;
                Console.WriteLine($"{ts.Seconds:00}:{ts.Milliseconds:00}");
                rest.AutomaticCancellationReservation();
            }
        }
    }
}