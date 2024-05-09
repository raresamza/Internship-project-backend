using WebApi.Config;
using WebApi.Middleware;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IEmailSenderService,EmailService>();

builder.Services.AddRepositories();
builder.Services.AddMediatR();
builder.Services.AddDbContext();
//builder.Services.AddJsonOptions();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseTiming();

//app.Use(async(ctx, next) =>
//{
//    var start = DateTime.UtcNow;
//    await next.Invoke(ctx);
//    app.Logger.LogInformation($"Request: {ctx.Request.Path}: {(DateTime.UtcNow-start).TotalMilliseconds}(ms)");
//});

//app.Use((HttpContext ctx, Func<Task> next) =>
//{
//    app.Logger.LogInformation("Terminating the Request");
//    return Task.CompletedTask;
//});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
