using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using WorkshopAdmin.Application; 
using WorkshopAdmin.Infrastructure;
using WorkshopAdmin.Infrastructure.Persistence;
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

var key = builder.Configuration["Jwt:key"];
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ClockSkew = TimeSpan.Zero,
        RoleClaimType = "role",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key!))
    };
});
builder.Services.AddAuthorization();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",policy =>
    {
        policy.WithOrigins("https://localhost:7004") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddScoped<DbSeeder>();


var app = builder.Build();

app.UseCors("AllowSpecificOrigins");

app.UseMiddleware<ExceptionHandlingMiddleware>();

// 4. Configurar el Pipeline (el orden importa aquí)
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();    // Mapea el JSON (/openapi/v1.json)
    app.UseSwagger();    // Genera el documento Swagger
    app.UseSwaggerUI();  // Crea la página visual en /swagger
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<DbSeeder>();
    await seeder.SeedAsync();
}

app.Run();