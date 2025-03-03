using Calendar.Services.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Calendar.BackGroundServices
{
    public class ReminderBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _services;

        public ReminderBackgroundService(IServiceProvider services)
        {
            _services = services;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                using (var scope = _services.CreateScope())
                {
                    var noteService = scope.ServiceProvider.GetRequiredService<INoteService>();
                    var hubContext = scope.ServiceProvider.GetRequiredService<IHubContext<NotificationHub>>();

                    var notes = await noteService.GetAllNotesAsync();
                    var now = DateTime.UtcNow;

                    foreach (var note in notes)
                    {
                        if (note.ReminderTime <= now && !note.IsNotified)
                        {
                            // Отправляем уведомление через SignalR
                            await hubContext.Clients.All.SendAsync("ReceiveNotification", note.Title);

                            // Помечаем заметку как уведомленную
                            note.IsNotified = true;
                            await noteService.UpdateNoteAsync(note);
                        }
                    }
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken); // Проверяем каждую минуту
            }
        }
    }
}
