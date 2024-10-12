using Microsoft.AspNetCore.Authentication.Certificate;
using System.ComponentModel.DataAnnotations;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços necessários à aplicação
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Adiciona o suporte para autenticação por certificado
builder.Services.AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
    .AddCertificate();

// Adiciona o suporte para controladores (para poder usar o controlador de produtos, por exemplo)
builder.Services.AddControllers();  // Aqui estamos dizendo que vamos usar controladores MVC

var app = builder.Build();

// Habilita a autenticação
app.UseAuthentication();

// Configura o pipeline de requisições HTTP
if (app.Environment.IsDevelopment())
{
    // Habilita o Swagger no ambiente de desenvolvimento
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Product API v1"));
}

app.UseHttpsRedirection();

// Habilita o roteamento e mapeia os controladores
app.UseRouting();
app.UseAuthorization();

app.MapControllers();  // Isso vai mapear automaticamente as rotas dos controladores

// Exemplo de endpoint de previsão do tempo
var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

// Classe auxiliar para o exemplo de previsão do tempo
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}

// Modelo de Produto para sua API
public class Product
{
    public int Id { get; set; }

    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; }

    [Range(0.01, 1000.00)]
    public double Price { get; set; }
}
