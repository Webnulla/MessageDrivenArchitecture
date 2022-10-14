using System;
using MassTransit;
using Messaging;

namespace RestaurantKitchen
{
    public class Manager
    {
        public Manager()
        {
            
        }

        public bool CheckKitchenReady(Guid orderId, Dish? dish)
        {
            return true;
        }
    }
}