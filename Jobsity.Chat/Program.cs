using Jobsity.Chat;
using Jobsity.Chat.BotQueue;
using Jobsity.Chat.CrossCutting.DependencyInjection;
using Jobsity.Chat.Data.Context;
using Jobsity.Chat.SignalR;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddDbContext<MyContext>(opt => opt.UseInMemoryDatabase("JobsityInMemoryDB"), ServiceLifetime.Singleton, ServiceLifetime.Singleton);
builder.Services.ConfigureDependenciesService();
builder.Services.ConfigureDependenciesRepository();

builder.Services.ConfigureQueue(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();

builder.Services.AddSignalR();
builder.Services.AddHealthChecks();

builder.Services.AddSingleton<BotQueueOperations>();
builder.Services.AddSingleton<ChatHub>();

var app = builder.Build();

app.ConfigureExceptionHandler();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseDefaultFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseCors();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
});

app.DataBaseFeed();
app.MapHub<ChatHub>("/chat");
app.UseRabbitListener();

app.Run();
