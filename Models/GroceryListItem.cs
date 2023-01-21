namespace Sams_Warehouse.Models
{
    public class GroceryListItem
    {
        public int GroceryListItemId { get; set; }
        public int GroceryListId { get; set; }
        public int ShoppingItemId { get; set; }

        public int Quantity { get; set; }

        // For navigation
        public ShoppingItem ShoppingItem { get; set; }
        public GroceryList GroceryList { get; set; }

    }
}
