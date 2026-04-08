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
//builder.WebHost.UseUrls("http://0.0.0.0:5000");

// =========================
// Controllers
// =========================

builder.Services.AddControllers();


// =========================
// Database
// =========================

//builder.Services.AddDbContext<TicketDbContext>(options =>
//    options.UseSqlServer(
//        builder.Configuration.GetConnectionString("TicketConnection")));
builder.Services.AddDbContext<TicketDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection")));




// =========================
// Repositories
// =========================

builder.Services.AddScoped<ITicketRepository, TicketRepository>();
builder.Services.AddScoped<IReopenPolicy, AppSettingsReopenPolicy>();


// =========================
// Use Cases
// =========================

builder.Services.AddScoped<CreateTicketUseCase>();
builder.Services.AddScoped<ReopenTicketUseCase>();
builder.Services.AddScoped<GetTicketByIdUseCase>();


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


app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";

        var error = context.Features.Get<Microsoft.AspNetCore.Diagnostics.IExceptionHandlerFeature>();

        if (error != null)
        {
            var response = new
            {
                message = "Ocurrio un error interno",
                detail = error.Error.Message // en prod esto se oculta
            };

            await context.Response.WriteAsJsonAsync(response);
        }
    });
});

// =========================
// Middleware
// =========================

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
    app.UseSwaggerUI();
//}


app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseResponseCaching();

app.UseAuthorization();

app.MapControllers();

app.MapGet("/health", () => Results.Ok("Healty"));

app.Run();