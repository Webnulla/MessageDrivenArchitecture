using System;
using System.Threading.Tasks;
using MassTransit;
using Messaging;

namespace RestarauntBooking.Consumers
{
    public class BookingRequestConsumer : IConsumer<IBookingRequest>
    {
        private readonly Restaurant _restaurant;

        public BookingRequestConsumer(Restaurant restaurant)
        {
            _restaurant = restaurant;
        }

        public async Task Consume(ConsumeContext<IBookingRequest> context)
        {
            Console.WriteLine($"[OrderId: {context.Message.OrderId}]");
            var result = await _restaurant.BookFreeTableAsync(1);
            
            await context.Publish<ITableBooked>(new TableBooked(context.Message.OrderId, result ?? false));
        }
    }
}