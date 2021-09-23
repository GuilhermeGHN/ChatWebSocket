using Microsoft.AspNetCore.Mvc;

namespace ChatWebSocket.Controllers
{
	public class ChatController : Controller
	{
		public IActionResult Index(string apelido = null)
		{
			if (string.IsNullOrWhiteSpace(apelido))
				return RedirectToAction("Index", "Home");

			ViewBag.Apelido = apelido;

			return View();
		}
	}
}
