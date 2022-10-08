using DataAccess.MongoDb.Abstract;
using DataAccess.MongoDb.Concrete;
using DataAccess.MongoDb.Models;
using RabbitMq;
using UserIdConsumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<StajProjectDbSettings>(ctx.Configuration.GetSection("StajProjectDb"));
        services.AddHostedService<Worker>();
        services.AddSingleton<ITicketDal, TicketDal>();
        services.AddSingleton<RabbitMqConsumerHelper>();
    })
    .Build();

await host.RunAsync();
