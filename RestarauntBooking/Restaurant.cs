using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Messaging;

namespace RestarauntBooking
{
    public class Restaurant
    {
        private readonly List<Table> _tables = new List<Table>();
        private readonly Producer _producer = new Producer("BookingNotification", "localhost");

        public Restaurant()
        {
            for (int i = 1; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }

        public void BookFreeTableAsync(int countOfPerson)
        {
            Console.WriteLine("Подождите секунду я подберу столик и подтвержу вашу бронь," +
                              "Вам придет уведомление");
            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPerson
                                                        && t.State == State.Free);
                await Task.Delay(1000 * 5); //у нас нерасторопные менеджеры, 5 секунд они находятся в поисках стола
                table?.SetState(State.Booked);
                
                _producer.Send(table is null 
                    ? $"УВЕДОМЛЕНИЕ: К сожалению, сейчас все столики заняты" 
                    : $"УВЕДОМЛЕНИЕ: Готово! Ваш столик номер {table.Id}");
            });
        }
    }
}