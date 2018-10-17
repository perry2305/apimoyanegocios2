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
    public class OrdersController : ApiController
    {
        private e_commerceApiContext db = new e_commerceApiContext();
        private MemoryOrderRepository orders;
        public OrdersController()
        {
            orders = new MemoryOrderRepository("DefaultEndpointsProtocol=https;AccountName=itcs98g5;AccountKey=2uRk0TPF1s4prbz3RdI7xhcTMeZq28eAdjWgmkjZzKaicbekfTEaSUzjFBLbSlEAZeTov//IKLzibchwiDaiAg==");
        }
        // GET: api/Orders
        public List<Order> GetOrders()
        {
            return orders.EveryOrder();
        }

        // GET: api/Orders/5
        [ResponseType(typeof(Order))]
        public Order GetOrder(int id)
        {
            Order order = orders.OrderById(id);
            return order;
        }

        // PUT: api/Orders/5
        //PUT: api/Products/5
        public Task<MyOwnResponse> Put([FromBody]Order orden)
        {
            return orders.UpdateOrder(orden);
        }

        // POST: api/Orders
        [ResponseType(typeof(Order))]
        public Task<MyOwnResponse> PostOrder([FromBody]Order order)
        {
            return orders.CreateOrder(order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Order))]
        public Task<MyOwnResponse> DeleteOrder(int id)
        {
            return orders.DeleteOrder(orders.OrderById(id));
        }

        [Route("api/Orders/{idOrder}/Wishlist")]
        [HttpPost]
        public async Task<MyOwnResponse> PutIntoWishlist(int idOrder)
        {
            return await orders.PutIntoWishList(idOrder);
        }

        [Route("api/Wishlist/{ClientId}")]
        [HttpGet]
        public List<Order> MyWishList(int ClientId)
        {
            return orders.MyWishList(ClientId);
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