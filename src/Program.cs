using System.Reflection;
using EscalaApi.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "EscalaApi",
        Version = "v2",
        Description = """
            Plataforma genérica de escalas.

            Fluxo típico: tipos → integrantes → estratégia → configuração → gerar preview → persistir.
            """
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
        c.IncludeXmlComments(xmlPath);
});
builder.Services.AddProjectServices(builder.Configuration);
builder.Services.AddControllers()
 .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
    });

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "EscalaApi v2");
            c.RoutePrefix = string.Empty;
        });
}

app.MapControllers();
app.Run();
