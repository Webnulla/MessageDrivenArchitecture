using System;

namespace Messaging
{
    public interface IKitchenAccident
    {
        public Guid OrderId { get; }
        
        public Dish Dish { get; }
    }
}