using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace RestaurantNotification
{
    public class Notifier
    {
        public void Notify(Guid orderId, Guid clientId, string message)
        {
            Console.WriteLine($"[OrderID: {orderId}] Уважаемый клиент {clientId}! {message}");
        }
    }
}