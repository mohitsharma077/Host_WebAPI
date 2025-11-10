
using Microsoft.EntityFrameworkCore;
using WebAPI.Data;

var builder = WebApplication.CreateBuilder(args);

// Read from configuration which DB to use
var usePostgres = builder.Configuration.GetValue<bool>("UsePostgres");

if (usePostgres)
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("PostgresConnection")));
}
else
{
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));
}

//builder.Services.AddDbContext<ApplicationDbContext>(options =>
//{
//    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
//    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
//});

builder.Services.AddSwaggerGen();
builder.Services.AddEndpointsApiExplorer();

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins("http://localhost:5173")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// IMPORTANT: Configure port for Render
var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Redirect root ("/") to Swagger UI ??
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

// Configure the HTTP request pipeline.

//app.UseHttpsRedirection();

app.UseAuthorization();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();
