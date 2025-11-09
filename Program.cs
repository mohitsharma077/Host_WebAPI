
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

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

// Redirect root ("/") to Swagger UI ??
app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
