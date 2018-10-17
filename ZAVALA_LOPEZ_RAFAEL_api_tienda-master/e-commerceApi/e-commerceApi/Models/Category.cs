using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace e_commerceApi.Models
{
    public class Category
    {
        [Key]
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }
        public String Description { get; set; }
        public int Products { get; set; }
    }


    public class CategoryEntity : TableEntity
    {
        public CategoryEntity(int CategoryId)
        {
            this.RowKey = CategoryId.ToString();
            this.PartitionKey = "Categorias";
        }
        public CategoryEntity() { }
        public int CategoryId { get; set; }
        public String CategoryName { get; set; }
        public String Description { get; set; }
        public int Products { get; set; }
    }
}