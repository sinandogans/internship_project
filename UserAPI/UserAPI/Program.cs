using DataAccess.MongoDb.Abstract;
using DataAccess.MongoDb.Concrete;
using DataAccess.MongoDb.Models;
using ServiceLayer.Utilities.Authentication;
using ServiceLayer.Utilities.Authorization;
using ServiceLayer.Utilities.JWT;
using ServiceLayer.Utilities.Validation.FluentValidation;
using Microsoft.OpenApi.Models;
using System.Reflection;
using ServiceLayer.Utilities.Validation;
using ServiceLayer.Utilities.MessageBroker.RabbitMQ;
using ServiceLayer.Utilities.Extensions.Middleware;
using ServiceLayer.Abstract;
using ServiceLayer.Concrete;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer.Utilities.MessageBroker.Kafka;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<StajProjectDbSettings>(builder.Configuration.GetSection("StajProjectDb"));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<IUserDal, UserDal>();
builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddSingleton<JwtHelper>();
builder.Services.AddSingleton<JwtValidator>();
builder.Services.AddSingleton<AuthenticationManager>();
builder.Services.AddSingleton<AuthorizationManager>();
builder.Services.AddSingleton<UserValidationManager>();
builder.Services.AddSingleton<UserValidator>();
builder.Services.AddSingleton<MqPublisherHelper>();
builder.Services.AddSingleton<MqConsumerHelper>();
//builder.Services.AddSingleton<KafkaConsumerHelper>();
builder.Services.AddSingleton<KafkaPublisherHelper>();

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressConsumesConstraintForFormFileParameters = true;
    options.SuppressInferBindingSourcesForParameters = true;
    options.SuppressModelStateInvalidFilter = true;
    options.SuppressMapClientErrors = true;
}); //modelstate arastir, custom model state yaz

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("staj_project_userapi", new OpenApiInfo
    {
        Title = "staj_project User API",
        Description = "An ASP.NET Core Web API for managing User entities for staj_project",
    });
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
//options.TokenValidationParameters = new()
//{
//    ValidateAudience = true,
//    ValidAudience = builder.Configuration["Token:Audience"],
//    ValidateIssuer = true,
//    ValidIssuer = builder.Configuration["Token:Audience"],
//    ValidateLifetime = true,
//    ValidateIssuerSigningKey = true,
//    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
//});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/staj_project_userapi/swagger.json", "staj_project_userapi");

    });
}
app.UseExceptionMiddleware();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
