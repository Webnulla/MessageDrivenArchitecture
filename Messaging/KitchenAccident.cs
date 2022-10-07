using System;

namespace Messaging
{
    public class KitchenAccident
    {
        public Guid OrderId { get; }
        
        public Dish Dish { get; }
    }
}