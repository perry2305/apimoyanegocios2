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
using Microsoft.Azure;

namespace e_commerceApi.Controllers
{
    public class ClientsViewController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private MemoryClientRepository clientes;
        // GET: ClientsView
        public ClientsViewController()
        {
            clientes = new MemoryClientRepository(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            clientes = Globals.ClientRepository;
        }
        public ActionResult Clientes()
        {
            return View(clientes.EveryClient());
        }

        public ActionResult Categorias()
        {
            MemoryCategoryRepository categorias = Globals.CategoryRepository;
            return View(categorias.EveryCategory());
        }
        public ActionResult Editar_Categoria(int idCategoria)
        {
            MemoryCategoryRepository categorias = Globals.CategoryRepository;
            return View(categorias.CategorybyId(idCategoria));
        }

        public ActionResult Registros()
        {
            return View(clientes.ShowRegisters());
        }

        public ActionResult Pagos()
        {
            MemoryPaymentRepository pagos = new MemoryPaymentRepository(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            return View(pagos.EveryPayment());
        }

        public async Task<ActionResult> DeletePayment(int idPago)
        {
            MemoryPaymentRepository pagos = new MemoryPaymentRepository(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var result = await pagos.DeletePayment(idPago);
            return RedirectToAction("Pagos");
        }

        public async Task<ActionResult> VerifyPayment(int idPago)
        {
            MemoryPaymentRepository pagos = new MemoryPaymentRepository(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var result = await pagos.VerifyPayment(idPago);
            return RedirectToAction("Pagos");
        }

        public async Task<ActionResult> AcceptRegister(int id)
        {
            MemoryClientRepository clientes = new MemoryClientRepository(CloudConfigurationManager.GetSetting("StorageConnectionString"));
            var res = await clientes.AcceptRegister(id);
            if(res.Result == "OK")
            {
                return RedirectToAction("Clientes");
            }else
            {
                return RedirectToAction("Registros");
            }
        }

        // GET: ClientsView/Details/5
        public ActionResult Details(int id)
        {
            Client client = clientes.ClientById(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: ClientsView/Edit/5
        public ActionResult Edit(int id)
        {
            Client client = clientes.ClientById(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: ClientsView/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "ClientId,Username,Password,CarritoId,UserType,InfoId")] Client client)
        {
            var res = await clientes.UpdateClient(client);
            if (res.Result == "OK")
            {
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: ClientsView/Delete/5
        public ActionResult Delete(int id)
        {
            Client client = clientes.ClientById(id);
            if (client == null)
            {
                return HttpNotFound();
            } 
            return View(client);
        }

        // POST: ClientsView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Client client = clientes.ClientById(id);
            var res = await clientes.DeleteClient(client);
            if(res.Result == "OK")
            {
                return RedirectToAction("Index");

            }
            else
            {
                return View();
            }
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
