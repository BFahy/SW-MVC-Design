using System.Collections.Generic;

namespace Sams_Warehouse.Models
{
    public class ShoppingItem
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public string Unit { get; set; }
        public decimal UnitPrice { get; set; }

        public ICollection<GroceryListItem> GroceryListItems { get; set; }

        public ShoppingItem()
        {

        }

        public ShoppingItem(int id, string itemName, string unit, decimal unitPrice)
        {
            Id = id;
            ItemName = itemName;
            Unit = unit;
            UnitPrice = unitPrice;
        }
    }
}
