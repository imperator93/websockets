using Microsoft.EntityFrameworkCore;
using FluentValidation;

using Api.Data;
using Api.Repository;
using Api.Services;
using Microsoft.Identity.Client;
using Api.Models;

var builder = WebApplication.CreateBuilder(args);
{

    builder.Services.AddDbContext<DataContext>(options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
    });

    builder.Services.AddControllers();
    builder.Services.AddCors(options =>
    {
        options.AddDefaultPolicy(policy =>
        {
            policy.AllowAnyHeader();
            policy.AllowAnyMethod();
            policy.AllowAnyOrigin();
        });
    });
    builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));

    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

    builder.Services.AddSingleton<TokenProvider>();
    builder.Services.AddTransient<EncryptionService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.MapControllers();
    app.UseCors();
    app.Run();
}