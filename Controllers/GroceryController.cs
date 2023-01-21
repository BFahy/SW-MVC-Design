using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Sams_Warehouse.Data;
using Sams_Warehouse.Models;

namespace Sams_Warehouse.Controllers
{
    public class GroceryController : Controller
    {
        /**
         * Global variable declaration - used for DB calls.
         */
        private readonly ShoppingContext _context;

        public GroceryController(ShoppingContext context)
        {
            _context = context;
        }

        /**
         * Returns page view for Grocery/Index.cshtml 
         */
        public IActionResult Index()
        {
            return View();
        }
        #region Partial Views
        /** _GroceryListDDL.cshtml
         * Populates dropdown list with grocery lists that match to userID.
         * Returns DDL with options related to grocery list name.
         */
        public async Task<IActionResult> GroceryListDDLPartial(int userID)
        {            

            var groceryLists = _context.GroceryList.Where(c => c.UserId == userID).ToList();
            

            var grocerySelectList = groceryLists.Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.GroceryListId.ToString(),
            }).ToList();

            ViewBag.GrocerySelectList = grocerySelectList;            

            return PartialView("_GroceryListDDL");
        }

        /** _GroceryListTablePartial.cshtml
         *  Fills table in Index with all information related to shopping items under grocery list.
         *  Returns partial view of tables with their relevant information.
         */
        public async Task<IActionResult> GroceryListTablePartial(int groceryListId)
        {
            //var grocery = await _context.GroceryListItems.Include(c => c.ShoppingItem)
            //                                             .Where(c => c.GroceryListId == groceryListId)
            //                                             .Select(c => c.ShoppingItem)
            //                                             .ToListAsync();

           
            var grocery = await _context.GroceryListItems.Include(c => c.ShoppingItem)
                                                         .Where(p => p.GroceryListId == groceryListId)
                                                         .Select(c => new ShoppingItemView()
                                                         {
                                                             Id = c.ShoppingItem.Id,
                                                             ItemName = c.ShoppingItem.ItemName,
                                                             Unit = c.ShoppingItem.Unit,
                                                             UnitPrice = c.ShoppingItem.UnitPrice,
                                                             Quantity = c.Quantity
                                                         }).ToListAsync();



            var date = await _context.GroceryList.Where(c => c.GroceryListId == groceryListId)
                .Select(c => c.DateCreated).FirstOrDefaultAsync();

            ViewBag.DateCreated = date.ToString();
            
            return PartialView("_GroceryListTablePartial", grocery);
        }

        /**
        * Calls database to provide all unadded items not currently added to selected list
        * Returns partial view to display in modal
        */
        public async Task<IActionResult> GetUnaddedGroceryList(int listId, string searchCriteria = "")
        {
            searchCriteria = searchCriteria == null ? "" : searchCriteria;

            var allItems = _context.ShoppingList.Where(c => c.ItemName.ToLower().StartsWith(searchCriteria.ToLower())).AsEnumerable();

            var addedItems = _context.GroceryListItems.Where(c => c.GroceryListId == listId)
                .Include(c => c.ShoppingItem)
                .Select(c => c.ShoppingItem)
                .AsEnumerable();

            var notSelectedItems = allItems.Except(addedItems);

            return PartialView("_GetUnaddedGroceryList", notSelectedItems);


        }


        #endregion

        #region CRUD Operations
        /**
         * Post request for creating a new grocery list linked to user Id.
         * Requires listName parameter to populate name of model.
         */
        [HttpPost]
        public async Task<IActionResult> AddNewGroceryList([FromQuery]int userId, [FromBody]string listName)
        {
            if (String.IsNullOrEmpty(listName) || userId == 0)
            {
                return BadRequest("Enter a list name before adding");
            }

            GroceryList newList = new GroceryList
            {
                Name = listName,
                UserId = userId,
                DateCreated = DateTime.Now
            };

            var existingList = _context.GroceryList.Where(c => c.Name.Equals(listName)).FirstOrDefault();

            if (existingList != null)
            {
                return BadRequest("A list with that name already exists");
            }
            try
            {
                _context.GroceryList.Add(newList);
                await _context.SaveChangesAsync();
                return Ok();
            } catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }


        /**
         * Post call made to add item as a grocery list item.
         */
        [HttpPost]
        public async Task<IActionResult> AddItemToGroceryList([FromBody] GroceryListItem item)
        {
            if (!ModelState.IsValid || item == null)
            {
                return BadRequest();
            }

            _context.GroceryListItems.Add(item);
            await _context.SaveChangesAsync();

            return Ok();
        }

        /**
         * Used to delete items from shopping list, requires id of item to delete and grocery list id to remove from.
         */
        [HttpDelete]
        public async Task<IActionResult> RemoveItemFromList(int itemId, int groceryListId)
        {
            var groceryListItem = _context.GroceryListItems.Where(c=>c.ShoppingItemId == itemId &&
            c.GroceryListId == groceryListId)
                .FirstOrDefault();

            if (groceryListItem == null)
            {
                return BadRequest();
            }

            _context.GroceryListItems.Remove(groceryListItem);
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion
    }
}
