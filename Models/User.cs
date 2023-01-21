using System.ComponentModel.DataAnnotations;

namespace Sams_Warehouse.Models
{
    public class User
    {
        public int Id { get; set; }
        [MinLength(4)]
        [MaxLength(30)]
        public string Email { get; set; }
        [MinLength(1)]
        [MaxLength(20)]
        public string Username { get; set; }
        public string Password { get; set; }

        public User()
        {

        }

        public User(string email, string username, string password)
        {
            Email = email; 
            Username = username;
            Password = password;  
        }
    }
}
