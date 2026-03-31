using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using TicketSystem.API.Middleware;
using TicketSystem.Application.Interfaces;
using TicketSystem.Application.UseCases;
using TicketSystem.Domain.Policies;
using TicketSystem.Infrastructure;
using TicketSystem.Infrastructure.Cache;
using TicketSystem.Infrastructure.Persistence;



var builder = WebApplication.CreateBuilder(args);

// =========================
// Controllers
// =========================

builder.Services.AddControllers();


// =========================
// Database
// =========================

builder.Services.AddDbContext<TicketDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("TicketConnection")));


// =========================
// Repositories
// =========================

builder.Services.AddScoped<ITicketRepository, TicketRepository>();


// =========================
// Use Cases
// =========================

builder.Services.AddScoped<CreateTicketUseCase>();
builder.Services.AddScoped<ReopenTicketUseCase>();


// =========================
// Domain Policies
// =========================

builder.Services.AddScoped<IReopenPolicy, AppSettingsReopenPolicy>();


// =========================
// Response Caching
// =========================
builder.Services.AddResponseCaching();

// =========================
// MemoryCache
// =========================
builder.Services.AddMemoryCache();
builder.Services.AddScoped<ICacheService, MemoryCacheService>();

// =========================
// Swagger
// =========================

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// =========================
// Middleware
// =========================
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
app.Run($"http://0.0.0.0:{port}");

//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
app.UseSwagger();
app.UseSwaggerUI();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.Run();