using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Sams_Warehouse.Models;
using Microsoft.Data.SqlClient;

namespace Sams_Warehouse.Data
{
    public class ShoppingContext : DbContext
    {
        private readonly IConfiguration configuration;

        public ShoppingContext(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(configuration.GetConnectionString("Default"));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        // Shopping List table set
        public DbSet<ShoppingItem> ShoppingList { get; set; }
        // Users table set
        public DbSet<User> Users { get; set; }
        // Grocery list table set
        public DbSet<GroceryList> GroceryList { get; set; }
        // Grocery list items table set
        public DbSet<GroceryListItem> GroceryListItems{ get; set; }

    }
}
