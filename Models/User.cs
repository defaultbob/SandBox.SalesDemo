using System;
using System.ComponentModel.DataAnnotations;
namespace SalesDemo.Models
{
    public class User
    {
        [Required]
        public string Session { get; set; }

        [Required]
        public string Domain { get; set; }

    }
}