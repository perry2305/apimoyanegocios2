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
    public class CartsController : ApiController
    {
        private e_commerceApiContext db = new e_commerceApiContext();
        private MemoryCartRepository carritos;


        public CartsController()
        {
            carritos = Globals.CartsRepository;
        }

        // GET: api/Carts
        public IEnumerable<Cart> GetCarts()
        {
            if(carritos.EveryCart().Count() > 0)
            {
                return carritos.EveryCart();
            } else
            {
                return Enumerable.Empty<Cart>();
            }
        }

        // GET: api/Carts/5
        [ResponseType(typeof(Cart))]
        public Cart GetCart(int id)
        {
            return carritos.CartById(id);
        }

        // PUT: api/Carts/5
        [ResponseType(typeof(Cart))]
        public async Task<MyOwnResponse> PutCart(Cart cart)
        {
            return await carritos.UpdateCart(cart);
        }

        // POST: api/Carts
        [ResponseType(typeof(Cart))]
        public Task<MyOwnResponse> PostCart(Cart cart)

        {
            return carritos.CreateCart(cart);
        }

        // DELETE: api/Carts/5
        [ResponseType(typeof(Cart))]
        public Task<MyOwnResponse> DeleteCart(int id)
        {
            return carritos.DeleteCart(carritos.CartById(id));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CartExists(int id)
        {
            return db.Carts.Count(e => e.CartId == id) > 0;
        }
    }
}