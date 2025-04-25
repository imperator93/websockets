using Microsoft.EntityFrameworkCore;
using FluentValidation;

using Api.Data;
using Api.Repository;
using Api.Services;

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
    builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
    builder.Services.AddTransient<EncryptionService>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();
    builder.Services.AddValidatorsFromAssemblies(AppDomain.CurrentDomain.GetAssemblies());
}

var app = builder.Build();
{
    app.MapControllers();
    app.UseCors();
    app.Run();
}