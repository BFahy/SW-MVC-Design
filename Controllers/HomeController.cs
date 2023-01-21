using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sams_Warehouse.Models;

namespace Sams_Warehouse.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        /**
         * Unused
         */
        public IActionResult Index()
        {
            return View();
        }

        /**
         * Returns page view for 'About us'
         */
        public IActionResult AboutUs()
        {
            return View();
        }
        /**
        * Unused
        */
        public IActionResult Settings()
        {
            return View();
        }

        /**
         * Returns page view for 'Privacy notice'
         */
        public IActionResult Privacy()
        {
            return View();
        }

        /**
         * Error page
         */
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
