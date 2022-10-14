﻿using System;
using MassTransit;
using Messaging;
using RestarauntBooking.Consumers;

namespace RestarauntBooking;

public sealed class RestaurantBookingSaga : MassTransitStateMachine<RestaurantBooking>
{
    public RestaurantBookingSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => BookingRequested,
            x =>
                x.CorrelateById(context => context.Message.OrderId)
                    .SelectId(context => context.Message.OrderId));

        Event(() => TableBooked,
            x =>
                x.CorrelateById(context => context.Message.OrderId));

        Event(() => KitchenReady,
            x =>
                x.CorrelateById(context => context.Message.OrderId));

        CompositeEvent(() => BookingApproved,
            x => x.ReadyEventStatus, KitchenReady, TableBooked);

        Event(() => BookingRequestFault,
            x =>
                x.CorrelateById(m => m.Message.Message.OrderId)
                    .SelectId(m => m.Message.Message.OrderId));

        Schedule(() => BookingExpired,
            x => x.ExpirationId, x =>
            {
                x.Delay = TimeSpan.FromSeconds(5);
                x.Received = e => e.CorrelateById(context => context.Message.OrderId);
            });

        Initially(
            When(BookingRequested)
                .Then(context =>
                {
                    context.Instance.CorrelationId = context.Data.OrderId;
                    context.Instance.OrderId = context.Data.OrderId;
                    context.Instance.ClientId = context.Data.ClientId;
                })
                .Schedule(BookingExpired,
                    context => new BookingExpire(context.Instance),
                    context => TimeSpan.FromSeconds(1))
                .TransitionTo(AwaitingBookingApproved)
        );


        During(AwaitingBookingApproved,
            When(BookingApproved)
                .Unschedule(BookingExpired)
                .Publish(context =>
                    (INotify)new Notify(context.Instance.OrderId, context.Instance.ClientId,
                        "Стол успешно забронирован"))
                .Finalize(),
            When(BookingExpired.Received)
                .Then(context => { Console.WriteLine("EXPIRED"); })
                .Publish(context => (INotify)new Notify(context.Message.OrderId, context.Message.OrderId, "asdasd"))
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }

    public MassTransit.State AwaitingBookingApproved { get; private set; }
    public Event<IBookingRequest> BookingRequested { get; private set; }
    public Event<ITableBooked> TableBooked { get; private set; }
    public Event<IKitchenReady> KitchenReady { get; private set; }
    public Event<Fault<IBookingRequest>> BookingRequestFault { get; private set; }

    public Schedule<RestaurantBooking, IBookingExpire> BookingExpired { get; private set; }
    public Event BookingApproved { get; private set; }
}