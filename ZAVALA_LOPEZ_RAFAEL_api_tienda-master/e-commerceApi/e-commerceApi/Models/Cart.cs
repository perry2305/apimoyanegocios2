using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace e_commerceApi.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public List<Order> Orders { get; set; }
        public int CartTotal { get; set; }
//        public string Status { get; set; }
    }
}