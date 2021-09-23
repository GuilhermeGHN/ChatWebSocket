using ChatWebSocket.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ChatWebSocket.Controllers
{
	public class HomeController : Controller
	{
		private readonly ConnectionManager _webSocketConnectionManager;

		public HomeController(ConnectionManager webSocketConnectionManager)
		{
			_webSocketConnectionManager = webSocketConnectionManager;
		}

		public IActionResult Index()
		{
			return View();
		}

		[HttpPost]
		public IActionResult Index(string apelido)
		{
			if (_webSocketConnectionManager.ApelidoExists(apelido))
			{
				ViewBag.Erro = "Já existe alguém conectado com este apelido, utilize outro.";
				return View();
			}

			return RedirectToAction("Index", "Chat", new { Apelido = apelido });
		}
	}
}
