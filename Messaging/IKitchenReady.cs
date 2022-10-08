﻿using System;

namespace Messaging
{
    public interface IKitchenReady
    {
        public Guid OrderId { get; }
    }

    public class KitchenReady : IKitchenReady
    {
        public KitchenReady(Guid orderId, bool ready)
        {
            OrderId = orderId;
        }

        public Guid OrderId { get; }
    }
}