using MassTransit;
using Messaging;

namespace Restaurant.Kitchen.Consumer;

public class BookingRequestFaultConsumer : IConsumer<Fault<IBookingRequest>>
{
    public Task Consume(ConsumeContext<Fault<IBookingRequest>> context)
    {
        Console.WriteLine($"[OrderId {context.Message.Message.OrderId}] Отмена на кухне");
        return Task.CompletedTask;
    }
}