using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace e_commerceApi.Models
{
    public class Client
    {
        [Key]
        public int ClientId { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public int CarritoId { get; set; }
        public string UserType { get; set; }
        public int InfoId { get; set; }
    }

    public class ClientInfo
    {
        [Key]
        public int InfoId { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public int ClientId { get; set; }
        public string Username { get; set; }
    }
}