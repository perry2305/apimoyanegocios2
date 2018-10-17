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
using System.Collections;
using Microsoft.Azure;

namespace e_commerceApi.Controllers
{
    public class UploadFileModel
    {
 
        public HttpPostedFileBase File{ get; set; }
        public Product Producto { get; set; }
        // Rest of model details
    }
    public class ProductsViewController : Controller
    {
        private e_commerceApiContext db = new e_commerceApiContext();
        private MemoryProductRepository products;

        // GET: ProductsView
        public ProductsViewController()
        {
            products = Globals.ProductsRepository;
        }

        public ActionResult Index()
        {
            return View(products.EveryProduct());
        }

        // GET: ProductsView/Details/5
        public ActionResult Details(int id)
        {
            Product product = products.ProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }


        public ActionResult ProductsByCategory(int idCategoria)
        {
            CategoryRepository c = Globals.CategoryRepository;
            return View(c.ProductByCategory(idCategoria));
        }

       
        // GET: ProductsView/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ProductsView/Create
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<ActionResult> Create(Product model)
        {
            if (model.Files[0] == null)
            {
                return View();
            }
            await products.CreateProduct(model);

            Product Foto = model;
            var urlFoto = await products.UpdateImage(model.Id, model.Files[0].FileName, model.Files[0].InputStream);
            
            if(urlFoto == "OK")
            {
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }
        /*
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Name,ShortDescription,LongDescription,DiscountPercent,IsOffer,PhotoUrl")] Product product)
        {
            if (ModelState.IsValid)
            {
                var res = await products.CreateProduct(product);
                if(res.Result == "OK")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return null;
                }
            }

            return View(product);
        }*/

        // GET: ProductsView/Edit/5
        public ActionResult Edit(int id)
        {
            Product product = products.ProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

       

        // POST: ProductsView/Edit/5
        // Para protegerse de ataques de publicación excesiva, habilite las propiedades específicas a las que desea enlazarse. Para obtener 
        // más información vea http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Product product)
        {
            Product lastProduct = products.ProductById(product.Id);
            product.PhotoUrl = lastProduct.PhotoUrl;
            product.Files = lastProduct.Files;
                
            var res = await products.UpdateProduct(product);
            if(res.Result == "OK")
            {
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.Message = "Failure";
                return View();
            }
        }

        // GET: ProductsView/Delete/5
        public  ActionResult Delete(int id)
        {
            Product product = products.ProductById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: ProductsView/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async  Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = products.ProductById(id);
            var res = await products.DeleteProduct(product);
            return RedirectToAction("Index");
        }
    }
}
