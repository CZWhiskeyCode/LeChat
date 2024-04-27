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

            if (userInDb != null)
            {
                return true;
            }
            else
            {
                return false; 
            }
        }
    }
}