using JusMonitor.Application.Common.Interfaces;
using JusMonitor.Infrastructure.ExternalServices.DataJud;
using JusMonitor.Infrastructure.Persistence;
using JusMonitor.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// EF Core + PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositórios
builder.Services.AddScoped<IAdvogadoRepository, AdvogadoRepository>();
builder.Services.AddScoped<IClienteRepository, ClienteRepository>();
builder.Services.AddScoped<IProcessoRepository, ProcessoRepository>();
builder.Services.AddScoped<IMovimentacaoRepository, MovimentacaoRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// MediatR
builder.Services.AddMediatR(cfg =>
    cfg.RegisterServicesFromAssembly(
        typeof(JusMonitor.Application.UseCases.Advogados.CadastrarAdvogado.CadastrarAdvogadoHandler).Assembly));

// DataJud HttpClient
builder.Services.AddHttpClient<IDataJudService, DataJudClient>(client =>
{
    client.BaseAddress = new Uri("https://api-publica.datajud.cnj.jus.br/");
    client.DefaultRequestHeaders.Add("Authorization",
        $"ApiKey {builder.Configuration["DataJud:ApiKey"]}");
});

// Scalar (documentação)
builder.Services.AddOpenApi();
builder.Services.AddControllers();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();