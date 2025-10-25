using Encurtador.Infrastructure; // Nosso método de DI
using Encurtador.WebApi.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Registrar Serviços ---

// Adiciona o Infrastructure (DbContext e Repositories)
builder.Services.AddInfrastructure(builder.Configuration);

// Adiciona nosso Serviço de lógica de negócio
builder.Services.AddScoped<UrlShorteningService>();

// Adiciona Controladores
builder.Services.AddControllers();

// Adiciona FluentValidation
builder.Services.AddFluentValidation(fv => 
    fv.RegisterValidatorsFromAssemblyContaining<Program>());

// Adiciona CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowNextApp", policy =>
    {
        policy.WithOrigins("http://localhost:3000") // URL do Next.js
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Adiciona Rate Limiting (Segurança contra Abuso)
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("fixed", opt =>
    {
        opt.PermitLimit = 10; // 10 requisições
        opt.Window = TimeSpan.FromMinutes(1); // por minuto
        opt.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Aplica o Rate Limiting
app.UseRateLimiter();

app.UseHttpsRedirection();

// Aplica o CORS
app.UseCors("AllowNextApp");

app.UseAuthorization();

// Mapeia os Controladores
// E aplica o rate limiting "fixed" a todos
app.MapControllers().RequireRateLimiting("fixed");

app.Run();