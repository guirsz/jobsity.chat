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

builder.Services.AddDbContext<MyContext>(opt => opt.UseInMemoryDatabase("JobsityInMemoryDB"));
builder.Services.ConfigureDependenciesService();
builder.Services.ConfigureDependenciesRepository();

builder.Services.ConfigureQueue(builder.Configuration);
builder.Services.ConfigureAuthentication(builder.Configuration);
builder.Services.ConfigureAuthorization();

builder.Services.AddSignalR();
builder.Services.AddHealthChecks();

//builder.Services.AddSingleton<ChatHub>();
builder.Services.AddSingleton<BotQueueOperations>();

var app = builder.Build();

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

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapHealthChecks("/health");
});

app.MapHub<ChatHub>("/chat");
app.UseRabbitListener();

app.Run();
