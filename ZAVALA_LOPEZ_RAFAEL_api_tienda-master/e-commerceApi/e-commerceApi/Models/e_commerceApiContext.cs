using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace e_commerceApi.Models
{
    public class e_commerceApiContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public e_commerceApiContext() : base("name=e_commerceApiContext")
        {
        }

        public System.Data.Entity.DbSet<e_commerceApi.Models.Product> Products { get; set; }

        public System.Data.Entity.DbSet<e_commerceApi.Models.Cart> Carts { get; set; }

        public System.Data.Entity.DbSet<e_commerceApi.Models.Category> Categoys { get; set; }

        public System.Data.Entity.DbSet<e_commerceApi.Models.Client> Clients { get; set; }

        public System.Data.Entity.DbSet<e_commerceApi.Models.Order> Orders { get; set; }
    }
}
