using e_commerceApi.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace e_commerceApi.Repositories
{
    interface CartRepository
    {
        Cart CartById(int id);
        List<Cart> EveryCart();
        Task<MyOwnResponse> CreateCart(Cart carrito);
        Task<MyOwnResponse> UpdateCart(Cart carrito);
        Task<MyOwnResponse> DeleteCart(Cart carrito);
    }

    public class MemoryCartRepository : CartRepository
    {
        static MemoryOrderRepository ordenes;
        public static int carsCounter = 0;
        private string ConnectionString;

        public MemoryCartRepository(string connectionString)
        {
            ConnectionString = Globals.connectionString;
            ordenes = Globals.OrdersRepository;
           

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Carritos");

            table.CreateIfNotExists();

            TableQuery <CartEntity> query = new TableQuery<CartEntity>();
            if (table.ExecuteQuery(query).Count() != 0)
            {
                IEnumerable<CartEntity> carritosEnTabla = table.ExecuteQuery(query).OrderByDescending(carritoEntity => carritoEntity.RowKey).ToList();
                int lastCartId = carritosEnTabla.First().CartId;
                carsCounter = lastCartId + 1;
            }
        }

        public Cart CartById(int id)
        {

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Carritos");
            TableOperation retrieveOperation = TableOperation.Retrieve<CartEntity>("Carritos", id.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                CartEntity resultEntity = (CartEntity)retrievedResult.Result;
                List<Order> orders = ordenes.OrderByCart(resultEntity.CartId);
                int totalCarrito = 0;
                foreach(Order o  in orders)
                {
                    totalCarrito = totalCarrito + o.Total;
                }
                Cart Result = new Cart()
                {
                    CartId = resultEntity.CartId,
                    CartTotal = totalCarrito,
                    Orders = ordenes.OrderByCart(resultEntity.CartId)
                };
                return Result;
            }
            else
            {
                return null;
            }
        }

        public void ChangeStatusCart(int idCart, string nvoStatus)
        {
            Cart carrito = CartById(idCart);
            Cart respaldo = carrito;
        }

        public async Task<MyOwnResponse> CreateCart(Cart carrito)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Carritos");

            table.CreateIfNotExists();

            var CarritoNuevo = new CartEntity(carrito.CartId);

            // CarritoNuevo.CartId = carsCounter;
            CarritoNuevo.CartId = carrito.CartId;
            CarritoNuevo.CartTotal = carrito.CartTotal;
            
            var insert = TableOperation.Insert(CarritoNuevo);
            var res = await table.ExecuteAsync(insert);

            if (res.Result != null)
            {
                carsCounter++;
                return new MyOwnResponse("OK", "El carrito se creó correctamente.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "El carrito no pudo crearse correctamente");
            }
        }

        public async Task<MyOwnResponse> UpdateCart(Cart carrito)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Carritos");
            TableOperation retrieveOperation = TableOperation.Retrieve<ProductoModelEntity>("Carritos", carrito.CartId.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                var UpdatedCart = new CartEntity(carrito.CartId);

                UpdatedCart.CartId = carrito.CartId;
                UpdatedCart.CartTotal = carrito.CartTotal;
                TableOperation updateOperation = TableOperation.Replace(UpdatedCart);
                var res = await table.ExecuteAsync(updateOperation);
                return new MyOwnResponse("OK","Se actualizó el carrito correctamente");
            }
            else
            {
                return new MyOwnResponse("ERROR","Problemas al actualizar el carrito");
            }

        }

        public async Task<MyOwnResponse> DeleteCart(Cart carrito)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Carritos");
            TableOperation retrieveOperation = TableOperation.Retrieve<ProductoModelEntity>("Carritos", carrito.CartId.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            CartEntity deleteEntity = (CartEntity)retrievedResult.Result;
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                var res = await table.ExecuteAsync(deleteOperation);
                return new MyOwnResponse("OK","Se elimino el carrito correctamente");
            }
            else
            {
                return new MyOwnResponse("ERROR","No pudimos eliminar el carrito");
            }
        }

        public List<Cart> EveryCart()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Carritos");

            TableQuery<CartEntity> query = new TableQuery<CartEntity>();
            List<Cart> lista = new List<Cart>();

            if (table.ExecuteQuery(query).Count() == 0)
            {
                return lista;
            }

            foreach (CartEntity entity in table.ExecuteQuery(query))
            {
                List<Order> ordenesPorCarrito = ordenes.OrderByCart(entity.CartId);
                Cart c = new Cart()
                {
                    CartId = entity.CartId,
                    CartTotal = ordenes.TotalCarrito(entity.CartId),
                    Orders = ordenesPorCarrito
                };
                lista.Add(c);
            }
            return lista;
        }

        public static implicit operator List<object>(MemoryCartRepository v)
        {
            throw new NotImplementedException();
        }
    }

    public class CartEntity : TableEntity
    {
        public CartEntity(int idCart)
        {
            this.RowKey = idCart.ToString();
            this.PartitionKey = "Carritos";
        }
        public int CartId { get; set; }
        public int CartTotal { get; set; }
        public CartEntity() { }
    }
}