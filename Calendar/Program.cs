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

// ���������� �������� � ��������� DI

// ��������� ������������ � OData
builder.Services.AddControllers()
    .AddOData(opt => opt.AddRouteComponents("odata", GetEdmModel())); // ��������� OData

// ��������� Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Calendar API", Version = "v1" });
});

// ��������� Entity Framework Core � PostgreSQL
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// ����������� ����������� � �������
builder.Services.AddScoped<INoteRepository, NoteRepository>();
builder.Services.AddScoped<INoteService, NoteService>();

// ��������� SignalR
builder.Services.AddSignalR();

// ����������� ������� ������ ��� �����������
builder.Services.AddHostedService<ReminderBackgroundService>();

var app = builder.Build();

// ��������� middleware

// ������������� Swagger
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

// ��������� ������������� ��� SignalR
app.MapHub<NotificationHub>("/notificationHub");

app.MapControllers();

app.Run();

// ����� ��� �������� EDM ������
static IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();
    builder.EntitySet<Note>("Notes"); // ������������ �������� Note ��� OData
    return builder.GetEdmModel();
}