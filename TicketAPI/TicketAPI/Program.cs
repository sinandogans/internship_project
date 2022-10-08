using DataAccess.MongoDb.Abstract;
using DataAccess.MongoDb.Concrete;
using DataAccess.MongoDb.Models;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ServiceLayer.Utilities.Extensions;
using ServiceLayer.Utilities.Validation;
using ServiceLayer.Utilities.Validation.FluentValidation;
using ServiceLayer.Abstract;
using ServiceLayer.Concrete;
using ServiceLayer.Utilities.MessageBroker.Kafka;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<StajProjectDbSettings>(builder.Configuration.GetSection("StajProjectDb"));

// Add services to the container.
builder.Services.AddSingleton<ITicketDal, TicketDal>();
builder.Services.AddSingleton<ITicketService, TicketService>();

builder.Services.AddSingleton<TicketValidationManager>();
builder.Services.AddSingleton<TicketValidator>();
builder.Services.AddSingleton<AnswerValidationManager>();
builder.Services.AddSingleton<AnswerValidator>();
//builder.Services.AddSingleton<MqPublisherHelper>();
//builder.Services.AddSingleton<MqConsumerHelper>();
builder.Services.AddSingleton<KafkaPublisherHelper>();
builder.Services.AddSingleton<KafkaConsumerHelper>();


//builder.Services.AddHostedService<DeleteAllTicketsByUserIdWorker>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("staj_project_ticketapi", new OpenApiInfo
    {
        Title = "staj_project Ticket API",
        Description = "An ASP.NET Core Web API for managing Ticket entities for staj_project",
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/staj_project_ticketapi/swagger.json", "staj_project_ticketapi");

    });
}
app.UseExceptionMiddleware();
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
