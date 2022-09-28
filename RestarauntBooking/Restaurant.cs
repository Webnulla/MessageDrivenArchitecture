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

        public Restaurant()
        {
            for (int i = 1; i <= 10; i++)
            {
                _tables.Add(new Table(i));
            }
        }

        public async Task<bool?> BookFreeTableAsync(int countOfPersons)
        {
            Console.WriteLine("Спасибо за Ваше обращение, я подберу столик и подтвержу вашу бронь," +
                              "Вам придет уведомление");

            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons
                                                    && t.State == State.Free);
            await Task.Delay(1000 * 5); //у нас нерасторопные менеджеры, 5 секунд они находятся в поисках стола
            return table?.SetState(State.Booked);
        }
    }
}