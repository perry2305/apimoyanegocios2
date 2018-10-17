using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using e_commerceApi.Models;
using e_commerceApi.Repositories;

namespace e_commerceApi.Controllers
{
    public class CategorysController : ApiController
    {
        private e_commerceApiContext db = new e_commerceApiContext();
        private MemoryCategoryRepository categorias;

        public CategorysController()
        {
            categorias = Globals.CategoryRepository;
        }
        // GET: api/Categorys
        public IEnumerable<Category> GetCategorys()
        {
            if(categorias.EveryCategory().Count() > 0)
            {
                return categorias.EveryCategory();
            } else
            {
                return Enumerable.Empty<Category>();
            }
        }

        // GET: api/Categorys/5
        [ResponseType(typeof(Category))]
        public Category GetCategory(int id)
        {
            return categorias.CategorybyId(id);
        }

        // PUT: api/Categorys/5
        public async Task<MyOwnResponse> Put(Category categoria)
        {
            return await categorias.UpdateCategory(categoria);  
        }

        // POST: api/Categorys
        public async Task<MyOwnResponse> Post(Category category)
        {
            return await categorias.CreateCategory(category);
        }

        // DELETE: api/Categorys/5
        public async Task<MyOwnResponse> DeleteCategoy(int id)
        {
            return await categorias.DeleteCategory(categorias.CategorybyId(id));
        }

        [Route("api/AddProductToCategory/{idProducto}/{idCategoria}")]
        [HttpGet]
        public async Task<MyOwnResponse> AddProductToCategory(int idProducto, int idCategoria)
        {
            return await categorias.AddProductToCategory(idProducto, idCategoria);
        }

        [Route("api/ProductsByCategory/{idCategoria}")]
        [HttpGet]
        public List<Product> ProductsByCategory(int idCategoria)
        {
            return categorias.ProductByCategory(idCategoria);
        }

        [Route("api/CategoriesByProduct/{idProducto}")]
        [HttpGet]
        public List<Category> CategoriesByProduct(int idProducto)
        {
            return categorias.CategoriesByProduct(idProducto);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}