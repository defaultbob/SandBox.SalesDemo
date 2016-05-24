using System;
using System.ComponentModel.DataAnnotations;
namespace SalesDemo.Models
{
    public class User
    {
        public User(){}
        public User(string session, string domain)
        {
            this.Session = session;
            this.Domain = domain;

            if (string.IsNullOrEmpty(this.Session) || string.IsNullOrEmpty(this.Domain))
            {
                throw new Exception("Must provide a session and a domain query string param - there is probably an issue with your Tab URL");
            }
        }

        [Required]
        public string Session { get; set; }

        [Required]
        public string Domain { get; set; }

    }
}