using Autofac;
using Autofac.Extensions.DependencyInjection;
using TQ.Core;
using TQ.Infrastructure.Data;
using TQ.UseCases;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(UseCasesAssembly).Assembly);


// Заменяем стандартный DI на Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
// Регистрируем модули Autofac
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>
{
    containerBuilder.RegisterModule<DefaultCoreModule>();
    containerBuilder.RegisterModule<DefaultInfrastructureModule>();
    // Здесь можно регистрировать и другие модули, если есть
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(UseCasesAssembly).Assembly);
});

builder.Services.AddTravelQuestDbContext(builder.Configuration.GetConnectionString("TravelQuestDbConnection"));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
