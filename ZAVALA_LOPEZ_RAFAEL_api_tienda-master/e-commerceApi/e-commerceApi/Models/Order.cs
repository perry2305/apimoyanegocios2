using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace e_commerceApi.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }
        public int Total { get; set; }
        public int ClientId { get; set; }
        public string Status { get; set; }
        public string ProductName { get; set; }
        public string ClientName { get; set; }
    }
}