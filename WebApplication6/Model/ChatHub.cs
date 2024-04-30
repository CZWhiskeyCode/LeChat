using Database;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace WebApplication6.Model
{
    public class ChatHub : Hub
    {
        private readonly ApplicationContext _context;

        public ChatHub(ApplicationContext context)
        {
            _context = context;
        }

        public async Task SendMessage(string user, string message)
        {
            if (IsUserAllowedToSendMessage(user))
            {
                await SaveMessage(user, message);
                await Clients.All.SendAsync("ReceiveMessage", user, message);
            }
            else
            {
                await Clients.Caller.SendAsync("ReceiveMessage", "Server", "You are not allowed to send messages.");
            }
        }

        private bool IsUserAllowedToSendMessage(string user)
        {
            var userInDb = _context.Users.FirstOrDefault(u => u.Login == user);

            if (userInDb != null && userInDb.Status == Status.Online)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private async Task SaveMessage(string user, string message)
        {
            try
            {
                var userInDb = _context.Users.FirstOrDefault(e => e.Login == user);

                if (userInDb != null)
                {
                    var newMessage = new Message
                    {
                        SenderId = userInDb.Id,
                        Content = message,
                        Timestamp = DateTime.Now
                    };
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving message:{ex.Message}");
            }

        }
    }
}