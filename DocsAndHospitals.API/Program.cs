using DocsAndHospitals.Auth;
using DocsAndHospitals.Models;
using DocsAndHospitals.Persistence;
using DocsAndHospitals.Services;
using DocsAndHospitals.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DocsAndHospitals.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Controllers & Swagger
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // DbContext (SQL Server)
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString, b => b.MigrationsAssembly("DocsAndHospitals.API")));

            // Dependency Injection
            builder.Services.AddScoped<IHospitalRepository>(provider =>
                new HospitalRepository("hospitals.json"));

            // HospitalService - Scoped 
            builder.Services.AddScoped<IHospitalService, HospitalService>();

            // Auth related dependencies
            builder.Services.AddScoped<IUserRepository, UserEfRepository>();
            builder.Services.AddScoped<AuthService>();
            builder.Services.AddSingleton<PasswordHasher>();

            // FluentValidation
            builder.Services.AddScoped<IValidator<RegisterRequest>, RegisterValidator>();
            builder.Services.AddScoped<IValidator<LoginRequest>, LoginValidator>();

            // JWT Settings
            builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
            var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
            builder.Services.AddSingleton(jwtSettings);
            builder.Services.AddSingleton<JwtService>();

            // JWT Authentication
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey))
                };
            });

            var app = builder.Build();

            using (var scope = app.Services.CreateScope())
            {
                var hospitalService = scope.ServiceProvider.GetRequiredService<IHospitalService>();
                await hospitalService.InitializeAsync();
            }

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();
            app.MapControllers();

            app.Run();
        }
    }
}
