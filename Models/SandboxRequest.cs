using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
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
        [VaultName]
        public string Name { get; set; }

        public string Description { get; set; }

        [Display(Name = "Target Domain Name")]
        public string TargetDomain { get; set; }

        public string Type { get { return "demo"; } }

        public string Status { get; set; }
    }

    public class VaultNameAttribute : ValidationAttribute
    {
        public readonly List<string> reservedWords = new List<string>();
        public VaultNameAttribute()
        {
            reservedWords.Add("login");
            reservedWords.Add("www");
            reservedWords.Add("mail");
            reservedWords.Add("help");
            reservedWords.Add("veeva");
            reservedWords.Add("test");
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string stringValue = value as String;
            if (!string.IsNullOrEmpty(stringValue))
            {
                string match = reservedWords.FirstOrDefault(s => s.Equals(stringValue.ToLower()));
                if (match != null)
                {
                    return new ValidationResult($"Cannot use reserved word {match}");
                }
            }

            return ValidationResult.Success;
        }
    }
}