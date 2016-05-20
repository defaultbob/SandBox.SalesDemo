using System;
using System.ComponentModel.DataAnnotations;
namespace SalesDemo.Models
{
    public class Login
    {
        [Required]
        public string Domain { get; set; }
        
        [Required]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

    }
}