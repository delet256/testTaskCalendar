using Microsoft.AspNetCore.SignalR;
namespace Calendar
{

    public class NotificationHub : Hub
    {
        // Метод для отправки уведомлений клиентам
        public async Task SendNotification(string message)
        {
            await Clients.All.SendAsync("ReceiveNotification", message);
        }
    }
}
