using System;
using System.Collections.Generic;

namespace Sams_Warehouse.Models
{
    public class GroceryList
    {
        public int GroceryListId { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public DateTime DateCreated { get; set; }

        // For navigation
        public User User { get; set; }
        public ICollection<GroceryListItem> GroceryListItems { get; set; }

    }
}
