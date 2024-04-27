//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.SignalR;
//using WebApplication6.Model;

//namespace WebApplication6.Controllers
//{
//    public class HomeController : Controller
//    {
//        private readonly IHubContext<ChatHub> _hubContext;

//        public HomeController(IHubContext<ChatHub> hubContext)
//        {
//            _hubContext = hubContext;
//        }

//        public IActionResult Index()
//        {
//            return View();
//        }

//        [HttpPost]
//        public async Task<IActionResult> SendMessage(string message)
//        {
//            await _hubContext.Clients.All.SendAsync("ReceiveMessage", message);
//            return RedirectToAction("Index");
//        }
//    }
//}
