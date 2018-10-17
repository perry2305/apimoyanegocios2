using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace e_commerceApi.Models
{
    public class Product
    {
        public int Id { get; set; }
        public int Price { get; set; }
        public String Name { get; set; }
        public String ShortDescription { get; set; }
        public String LongDescription { get; set; }
        public int DiscountPercent { get; set; }
        public Boolean IsOffer { get; set; }
        public String PhotoUrl { get; set; }
        
        public List<HttpPostedFileBase> Files { get; set; }
    }

    public class CategoryProduct
    {
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
    }

    public class CategoryProductEntity : TableEntity
    {
        public CategoryProductEntity(int id)
        {
            this.RowKey = id.ToString();
            this.PartitionKey = "CategoriaProducto";
        }
        public int CategoryId { get; set; }
        public int ProductId { get; set; }
        public CategoryProductEntity() { }
    }
}