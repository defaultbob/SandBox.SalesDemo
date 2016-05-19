using System;
using System.ComponentModel.DataAnnotations;
namespace SalesDemo.Models
{
    public class SandboxRequest
    {
        public SandboxRequest()
        {
        }
        public SandboxRequest(User user)
        {
            this.User = user;
        }

        public User User { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        public string TargetDomain { get; set; }

        public string Type { get { return "Demo"; } }

        public string Status { get; set; }
    }
}