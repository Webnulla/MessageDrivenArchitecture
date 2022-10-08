using System;
using System.Threading.Tasks;
using MassTransit;
using Messaging;

namespace RestaurantKitchen.Consumers;

public class TableBookedConsumer : IConsumer<IBookingRequest>
{
    private readonly Manager _manager;

    public TableBookedConsumer(Manager manager)
    {
        _manager = manager;
    }
        
    public async Task Consume(ConsumeContext<IBookingRequest> context)
    {
        Console.WriteLine($"[OrderId: {context.Message.OrderId} CreationDate: {context.Message.CreationDate}]");
        Console.WriteLine("Trying time: " + DateTime.Now);
                
        if(_manager.CheckKitchenReady(context.Message.OrderId, context.Message.PreOrder))
            await context.Publish<IKitchenReady>(new KitchenReady(context.Message.OrderId, true));
    }
}