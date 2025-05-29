using NotificationApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton(new SlidingWindowRateLimiter(10, TimeSpan.FromMinutes(1)));
builder.Services.AddSingleton<IExternalNotifier, DiscordNotifier>();
builder.Services.AddScoped<INotificationService, NotificationService>();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/", () => "Welcome to Notification API!");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
