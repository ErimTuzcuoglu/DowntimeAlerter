using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using DowntimeAlerter.Infrastructure.ViewModel.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using DowntimeAlerter.Models;
using DowntimeAlerter.Services.Contract;
using Microsoft.AspNetCore.Http;

namespace DowntimeAlerter.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _service;
        private readonly IMapper _mapper;

        public HomeController(ILogger<HomeController> logger, IUserService service, IMapper mapper)
        {
            _logger = logger;
            _service = service;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model.UserName == String.Empty || model.Password == String.Empty)
            {
                ViewBag.error = "You should fill the fields";
                return View("Error");
            }

            var result = await _service.Login(model);
            if (!result.Succeeded) return View("Index");

            var cookie = new CookieOptions();
            cookie.Expires = DateTimeOffset.Now.AddMinutes(60001);
            Response.Cookies.Append("daTkn", result.Data.AccessToken, cookie);
            
            HttpContext.Session.SetString("UserName", result.Data.User.UserName);
            return View("Index");
        }

        [Route("logout")]
        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}