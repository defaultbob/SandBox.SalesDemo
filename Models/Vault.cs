using System;
using System.ComponentModel.DataAnnotations;
namespace SalesDemo.Models
{
    public class Vault
    {
        public int Id { get; set; }
        
        [DataType(DataType.Url)]
        public string Dns { get; set; }
        
        public string Message { get; set; }
    }
}