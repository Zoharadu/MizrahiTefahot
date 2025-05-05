using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Service.ImpI;
using Service.Interface;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()  // או אפשר להשתמש ב־ .WithOrigins("https://editor.swagger.io")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // אם לא משתמשים ב-HTTPS בסביבה שלך
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidIssuer = "your-issuer", // לשים את ה-issuer שלך כאן
            ValidAudience = "your-audience", // לשים את ה-audience שלך כאן
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("your-very-strong-secret-key-128-bits-long!")) // מפתח סודי שלך
        };
    });

builder.Services.AddSwaggerGen(options =>
{
    // הוספת אפשרות עבור טוקן JWT
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Description = "Please enter the JWT with Bearer prefix (e.g., 'Bearer your_token')"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<ICalculationService, CalculationService>();

var app = builder.Build();

app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Calculation API v1");
        options.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
