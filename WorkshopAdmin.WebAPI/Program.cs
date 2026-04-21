using WorkshopAdmin.Application; 
using WorkshopAdmin.Infrastructure;
using WorkshopAdmin.WebAPI.Middleware;

var builder = WebApplication.CreateBuilder(args);

// 1. Servicios básicos
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer(); // Necesario para que Swagger encuentre tus endpoints

// 2. Configuración de OpenAPI y Swagger
builder.Services.AddOpenApi();     
builder.Services.AddSwaggerGen();  // La parte de Swashbuckle para generar el UI

// 3. Tus capas de Clean Architecture
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();

var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

// 4. Configurar el Pipeline (el orden importa aquí)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();    // Mapea el JSON (/openapi/v1.json)
    app.UseSwagger();    // Genera el documento Swagger
    app.UseSwaggerUI();  // Crea la página visual en /swagger
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();