using EscalaApi.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddProjectServices(builder.Configuration);
builder.Services.AddMappingServices();
builder.Services.AddControllers();



var app = builder.Build();
    
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "Escala API V1");
            c.RoutePrefix = string.Empty; // Redireciona para Swagger automaticamente
        });
}

// app.UseHttpsRedirection();
app.MapControllers();
app.Run();
