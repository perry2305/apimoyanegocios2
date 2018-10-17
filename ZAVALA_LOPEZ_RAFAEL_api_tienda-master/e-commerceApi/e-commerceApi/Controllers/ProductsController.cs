using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using e_commerceApi.Models;
using e_commerceApi.Repositories;
using System.Web;

namespace e_commerceApi.Controllers
{
    
    public class ProductsController : ApiController
    {
        private e_commerceApiContext db = new e_commerceApiContext();
        private MemoryProductRepository products;
        public ProductsController()
        {
            products = Globals.ProductsRepository;
        }

        // GET: api/Products
        public IEnumerable<Product> Get()
        {
            if (products.EveryProduct().Count > 0)
            {
                return products.EveryProduct();
            }
            else
            {
                return Enumerable.Empty<Product>();
            }
        }
        // GET: api/Products/5
        public Product GetProduct(int id)

        {
            return products.ProductById(id);
        }

        //POST: api/Products

        [HttpPost]
        public async Task<MyOwnResponse> Post([FromBody]Product model)
        {
            return await products.CreateProduct(model);
        }

        //PUT: api/Products/5
        public async Task<MyOwnResponse> Put([FromBody]Product product)
        {
            return await products.UpdateProduct(product);
        }

        //DELETE api/Products/5
        public async Task<MyOwnResponse> Delete(int id)
        {
            Product productoDelete =  products.ProductById(id);
            if(productoDelete == null)
            {
                return new MyOwnResponse("ERROR","No pudimos encontrar el producto, verifica la información.");
            }
            return await products.DeleteProduct(productoDelete);
        }
                
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.Id == id) > 0;
        }
    }
}