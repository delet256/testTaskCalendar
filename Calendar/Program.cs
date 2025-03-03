using Calendar;
using Calendar.BackGroundServices;
using Calendar.Models;
using Calendar.Repositories;
using Calendar.Repositories.Interfaces;
using Calendar.Services;
using Calendar.Services.Interfaces;
using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер DI

// Настройка контроллеров и OData
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel())); // Настройка OData

// Настройка Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Calendar API", Version = "v1" });
});

// Настройка Entity Framework Core и PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Регистрация репозитория и сервиса
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

// Настройка SignalR
builder.Services.AddSignalR();

// Регистрация фоновой задачи для уведомлений
builder.Services.AddHostedService<ReminderBackgroundService>();

var app = builder.Build();

// Настройка middleware

// Использование Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Calendar API v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();

// Настройка маршрутизации для SignalR
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();

app.Run();

// Метод для создания EDM модели
static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Note>("Notes"); // Регистрируем сущность Note для OData
    return builder.GetEdmModel();
}