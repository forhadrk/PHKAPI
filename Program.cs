using Microsoft.EntityFrameworkCore;
using PHKAPI;
using PHKAPI.JwtTokens;
using PMS.API.DBContext;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<DatabaseContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection")));

builder.Services.AddScoped<IDapper, Dapperr>();
// Add services to the container.

builder.Services.AddControllers();

var PMSAllowOrigin = "_myPMSOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: PMSAllowOrigin,
        builder =>
        {
            builder.WithOrigins("http://localhost:4200",
                "https://api.perthhousekeeping.com.au",
                "https://perthhousekeeping.com.au",
                "http://api.perthhousekeeping.com.au",
                "http://perthhousekeeping.services",
                "http://perthhousekeeping.com.au"
                ) // Update with your Angular application's origin
                   .AllowAnyMethod()
                   .AllowAnyHeader()
                   .AllowCredentials();
        });
});

builder.Services.AddSingleton<IJWTManagerRepository, JWTManagerRepository>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors(PMSAllowOrigin);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
