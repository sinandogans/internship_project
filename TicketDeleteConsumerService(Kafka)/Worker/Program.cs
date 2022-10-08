using DataAccess.MongoDb.Abstract;
using DataAccess.MongoDb.Concrete;
using DataAccess.MongoDb.Models;
using Worker.MessageBroker.Kafka;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((ctx, services) =>
    {
        services.Configure<StajProjectDbSettings>(ctx.Configuration.GetSection("StajProjectDb"));
        services.AddSingleton<KafkaConsumerHelper>();
        services.AddSingleton<ITicketDal, TicketDal>();
        services.AddHostedService<Worker.Worker>();
    })
    .Build();

await host.RunAsync();
