using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web;
using System.Web.Mvc;
using e_commerceApi.Models;
using e_commerceApi.Repositories;

namespace e_commerceApi.Controllers
{
    public class OrdersViewController : Controller
    {
        private e_commerceApiContext db = new e_commerceApiContext();
        private MemoryOrderRepository orders;
        // GET: OrdersView
        public OrdersViewController()
        {
            orders = new MemoryOrderRepository("DefaultEndpointsProtocol=https;AccountName=itcs98g5;AccountKey=2uRk0TPF1s4prbz3RdI7xhcTMeZq28eAdjWgmkjZzKaicbekfTEaSUzjFBLbSlEAZeTov//IKLzibchwiDaiAg==");
        }
        public ActionResult Index()
        {
            return View(orders.EveryOrder());
        }

        // GET: OrdersView/Details/5
        public ActionResult Details(int id)
        {
            Order order = orders.OrderById(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: OrdersView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: OrdersView/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Order order)
        {
            if (ModelState.IsValid)
            {
                orders.CreateOrder(order);
                return RedirectToAction("Index");
            }

            return View(order);
        }

        // GET: OrdersView/Edit/5
        public ActionResult Edit(int id)
        {
            Order order = orders.OrderById(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: OrdersView/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Edit([Bind(Include = "OrderId,ProductId,Quantity,CarritoId,Total,ClientId")] Order order)
        {
            if (ModelState.IsValid)
            {
                orders.UpdateOrder(order);
                return RedirectToAction("Index");
            }
            return View(order);
        }

        // GET: OrdersView/Delete/5
        public ActionResult Delete(int id)
        {
            Order order = orders.OrderById(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: OrdersView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = orders.OrderById(id);
            orders.DeleteOrder(order);
            return RedirectToAction("Index");
        }
    }
}
