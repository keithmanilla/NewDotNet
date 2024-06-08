using DotNetAPI.Services;
using DotNetAPI.Models;
using Serilog;
using System.Diagnostics;


var builder = WebApplication.CreateBuilder(args);

// Add from the line below
var LocalReactDevelopment = "_localReactDevelopment";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: LocalReactDevelopment,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:3000")
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                        //   .AllowAnyOrigin();
                      });
});
// to the line above

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<LLMDatabaseSettings>(
    builder.Configuration.GetSection("LLMDatabase"));

builder.Services.AddControllers()
    .AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);

builder.Services.AddSingleton<BooksService>();

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateLogger();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Use(async (context, next) =>
{
    var httpMethod = context.Request.Method;
    var stopwatch = Stopwatch.StartNew();
    try
    {
        await next.Invoke();
    }
    finally
    {
        stopwatch.Stop();
        var duration = stopwatch.ElapsedMilliseconds;
        Log.Information("{Method} {Path}. {Duration} ms", httpMethod, context.Request.Path, duration);
    }
});

// This publishes the controller.
app.MapControllers();

// Add this
app.UseCors(LocalReactDevelopment);

app.Run();
