﻿using MassTransit;
using MassTransit.Testing;
using Messaging;
using Messaging.InMemoryDb;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using RestarauntBooking;
using RestarauntBooking.Consumers;

namespace RestaurantTests;

public class FakeRepository<T> : IInMemoryRepository<T> where T : class {
    public void AddOrUpdate(T entity)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<T> Get()
    {
        throw new NotImplementedException();
    }
}


[TestFixture]
public class ConsumerTests
{
    private ServiceProvider _provider;
    private ITestHarness _harness;

    [OneTimeSetUp]
    public async Task Init()
    {
        _provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddConsumer<BookingRequestConsumer>();
            })
            .AddLogging()
            .AddTransient<RestaurantBooking>()
            .AddSingleton<IInMemoryRepository<IBookingRequest>, FakeRepository<IBookingRequest>>()
            .BuildServiceProvider(true);
        
        _harness = _provider.GetTestHarness();

        await _harness.Start();
    }

    [OneTimeTearDown]
    public async Task TearDown()
    {
        await _harness.OutputTimeline(TestContext.Out, options => options.Now().IncludeAddress());
        await _provider.DisposeAsync();
    }
    
    
    [Test]
    public async Task Any_booking_request_consumed()
    {
        var orderIdd = Guid.NewGuid();
        
        await _harness.Bus.Publish(
            (IBookingRequest) new BookingRequest(
                orderIdd,
                Guid.NewGuid(),
                null,
                DateTime.Now));

        Assert.That(await _harness.Consumed.Any<IBookingRequest>());
    }

    [Test]
    public async Task Booking_request_consumer_published_table_booked_message()
    {
        var consumer = _harness.GetConsumerHarness<BookingRequestConsumer>();
        
        var orderId = NewId.NextGuid();
        var bus = _harness.Bus;
        
        await bus.Publish((IBookingRequest)
            new BookingRequest(orderId,
                orderId,
                null,
                DateTime.Now));

        Assert.That(consumer.Consumed.Select<IBookingRequest>()
            .Any(x => x.Context.Message.OrderId == orderId), Is.True);
        
        Assert.That(_harness.Published.Select<ITableBooked>()
            .Any(x => x.Context.Message.OrderId == orderId), Is.True);
    }
}