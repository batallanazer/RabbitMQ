﻿using CrossCuttingLayer;
using MassTransit;
using Microservice.RabbitMQ.Consumer;

Console.Title = "Notification";
var bus = Bus.Factory.CreateUsingRabbitMq(cfg =>
{
    cfg.Host(new Uri(RabbitMqConsts.RabbitMqRootUri), h =>
    {
        h.Username(RabbitMqConsts.UserName);
        h.Password(RabbitMqConsts.Password);
    });

    cfg.ReceiveEndpoint("todoQueue", ep => 
    {
        ep.PrefetchCount = 16;
        ep.UseMessageRetry(r => r.Interval(2, 100));
        ep.Consumer<TodoConsumerNotification>();
    });
});
bus.StartAsync();
Console.WriteLine("Listening for Todo registered events.. Press enter to exit");
Console.ReadLine();
bus.StopAsync();
