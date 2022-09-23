using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MessageDrivenArchitecture
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

        public void BookFreeTable(int countOfPersons)
        {
            Console.WriteLine("Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, оставатесь на линии");

            var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons && t.State == State.Free);
            
            Thread.Sleep(1000 * 5); // поиск столика

            Console.WriteLine(table is null 
            ? $"К сожалению, сейчас все столики заняты"
            : $"Готово! Ваш столик номер {table.Id}");
        }

        public void BookFreeTableAsync(int countOfPersons)
        {
            Console.WriteLine(
                "Добрый день! Подождите секунду я подберу столик и подтвержу вашу бронь, Вам придет уведомление");
            Task.Run(async () =>
            {
                var table = _tables.FirstOrDefault(t => t.SeatsCount > countOfPersons && t.State == State.Free);
                await Task.Delay(1000 * 5);
                table?.SetState(State.Booked);
                
                Console.WriteLine(table is null 
                    ? $"УВЕДОМЛЕНИЕ: К сожалению, сейчас все столики заняты"
                    : $"УВЕДОМЛЕНИЕ: Готово! Ваш столик номер {table.Id}");
            });
        }

        public void CancelReservation(int tableNumber)
        {
            var table = _tables.Find(t => t.Id == tableNumber && t.State == State.Booked);
            _tables.Remove(table);
            Thread.Sleep(1000 * 5);
            Console.WriteLine("Бронь снята");
        }

        public void CancelReservationAsync(int tableNumber)
        {
            Task.Run(async () =>
            {
                var table = _tables.Find(t => t.Id == tableNumber && t.State == State.Booked);
                _tables.Remove(table);
                await Task.Delay(1000 * 5);
                Console.WriteLine("Бронь снята");
            });
        }

        public void CancellationReservation(object state)
        {
            var table = _tables.Find(t => t.State == State.Booked);
            _tables.Remove(table);
        }
        //автоматическая отмена брони
        public void AutomaticCancellationReservation()
        {
            TimerCallback timerCallback = new TimerCallback(CancellationReservation);
            Timer timer = new Timer(timerCallback, null, 20000, 0);
        }
    }
}