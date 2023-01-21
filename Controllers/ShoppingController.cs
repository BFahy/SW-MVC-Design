using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Sams_Warehouse.Data;
using Sams_Warehouse.Models;

namespace Sams_Warehouse.Controllers
{
    public class ShoppingController : Controller
    {
        /**
         * Global variable declaration - used for DB calls.
         */
        private readonly ShoppingContext _context;

        public ShoppingController(ShoppingContext context)
        {
            _context = context;
        }
     
        /**
         * Awaits function call to display catalogue entries
         */
        public async Task<IActionResult> Index()
        {
            return View(await GetShoppingData());
        }

        /**
         * Provides partial view for all entries as partial view under _ShoppingTable
         */
        public async Task<IActionResult> ShoppingTable()
        {
            await Task.Delay(3000);
            return PartialView("_ShoppingTable", await GetShoppingData());
        }

        /**
         * Used to provide full list of catalogue items on home page
         */        
        private async Task<List<ShoppingItem>> GetShoppingData()
        {
            return await _context.ShoppingList.ToListAsync();
        }



    }
}
