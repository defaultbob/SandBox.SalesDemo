using System;
using System.ComponentModel.DataAnnotations;
namespace SalesDemo.Models
{
    public class Job
    {
        public int Id { get; set; }
        
        public string Status { get; set; }

        public string Results { get; set; }

    }
}
