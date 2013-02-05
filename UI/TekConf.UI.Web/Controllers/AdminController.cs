﻿using System.Configuration;
using System.Web.Mvc;

namespace TekConf.UI.Web.Controllers
{
	[Authorize]
	public class AdminController : Controller
	{
		private RemoteDataRepositoryAsync _repository;
		public AdminController()
		{
			var baseUrl = ConfigurationManager.AppSettings["BaseUrl"];
			_repository = new RemoteDataRepositoryAsync(baseUrl);
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}