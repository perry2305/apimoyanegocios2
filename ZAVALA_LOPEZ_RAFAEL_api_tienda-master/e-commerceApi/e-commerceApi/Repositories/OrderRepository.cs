using e_commerceApi.Models;
using e_commerceApi.Repositories;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace e_commerceApi.Repositories
{
    
    public static class Globals
    {
       
        
        public static string connectionString = "DefaultEndpointsProtocol=https;AccountName=itcs98g5;AccountKey=2uRk0TPF1s4prbz3RdI7xhcTMeZq28eAdjWgmkjZzKaicbekfTEaSUzjFBLbSlEAZeTov//IKLzibchwiDaiAg==";
        public static MemoryProductRepository ProductsRepository = new MemoryProductRepository(connectionString);
        public static MemoryOrderRepository OrdersRepository = new MemoryOrderRepository(connectionString);
        public static MemoryCartRepository CartsRepository = new MemoryCartRepository(connectionString);
        public static MemoryClientRepository ClientRepository = new MemoryClientRepository(connectionString);
        public static MemoryCategoryRepository CategoryRepository = new MemoryCategoryRepository(connectionString);
        public static MemoryPaymentRepository PaymentRepository = new MemoryPaymentRepository(connectionString);
        
    }

    public class MyOwnResponse
    {
        public string Result;
        public string Messagge;
        public MyOwnResponse(string result, string message)
        {
            Result = result;
            Messagge = message;
        }
    }

    interface OrderRepository
    {
        Order OrderById(int id);
        List<Order> EveryOrder();
        Task<MyOwnResponse> CreateOrder(Order nuevaOrden);
        Task<MyOwnResponse> UpdateOrder(Order orden);
        Task<MyOwnResponse> DeleteOrder(Order orden);
        Task<MyOwnResponse>PutIntoWishList(int idOrder);
        List<Order> OrderByCart(int idCart);
        List<Order> MyWishList(int idClient);
        int TotalCarrito(int idCart);
    }



    public class MemoryOrderRepository : OrderRepository
    { 
        public int ordersCounter = 0;
        private string ConnectionString;
        MemoryProductRepository productRepository;
        MemoryCartRepository CartRepository;
        MemoryClientRepository ClientRepository;

        public MemoryOrderRepository(string connectionString)
        {
            productRepository = Globals.ProductsRepository;
            CartRepository = Globals.CartsRepository;
            ClientRepository = Globals.ClientRepository;
            this.ConnectionString = Globals.connectionString;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Ordenes");

            table.CreateIfNotExists();

            TableQuery<OrderEntity> query = new TableQuery<OrderEntity>();
            if (table.ExecuteQuery(query).Count() != 0)
            {
                List<OrderEntity> OrdenesEnTabla = table.ExecuteQuery(query).OrderByDescending(order => order.RowKey).ToList();
                int lastOrderId = Int32.Parse(OrdenesEnTabla.First().RowKey);
                ordersCounter = lastOrderId + 1;
            }
        }
        

        public Order OrderById(int id)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Ordenes");
            TableOperation retrieveOperation = TableOperation.Retrieve<OrderEntity>("Ordenes",id.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                OrderEntity resultEntity = (OrderEntity)retrievedResult.Result;
                Order Result = new Order()
                {
                    OrderId = resultEntity.OrderId,
                    CartId = resultEntity.CartId,
                    ClientId = resultEntity.ClientId,
                    ProductId = resultEntity.ProductId,
                    Quantity = resultEntity.Quantity,
                    Status = resultEntity.Status,
                    Total = resultEntity.Total,
                    ProductName = productRepository.ProductById(resultEntity.ProductId).Name,
                    ClientName = ClientRepository.ClientById(resultEntity.ClientId).Username
                };
                return Result;
            }
            else
            {
                return null;
            }
        }

        public List<Order> EveryOrder()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Ordenes");

            TableQuery<OrderEntity> query = new TableQuery<OrderEntity>();
            List<Order> lista = new List<Order>();
            if(table.ExecuteQuery(query).Count() == 0)
            {
                return lista;
            }
            foreach (OrderEntity entity in table.ExecuteQuery(query))
            {
                Order orden = new Order()
                {
                    OrderId = entity.OrderId,
                    CartId = entity.CartId,
                    ClientId = entity.ClientId,
                    ProductId = entity.ProductId,
                    Quantity = entity.Quantity,
                    Status = entity.Status,
                    Total = entity.Total,
                    ProductName = productRepository.ProductById(entity.ProductId).Name,
                    ClientName = ClientRepository.ClientById(entity.ClientId).Username
                };
                lista.Add(orden);
                lista.OrderBy(ord => ord.OrderId).ToList();
            }
            return lista;
        }


        public async Task<MyOwnResponse> CreateOrder(Order nuevaOrden)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Ordenes");

            table.CreateIfNotExists();

            OrderEntity OrdenNueva = new OrderEntity(ordersCounter);
            OrdenNueva.OrderId = ordersCounter;
            OrdenNueva.ProductId = nuevaOrden.ProductId;
            OrdenNueva.Quantity = nuevaOrden.Quantity;

            Product productoOrden = productRepository.ProductById(nuevaOrden.ProductId);
            double restandoPorcentaje = (productoOrden.Price * (100 - productoOrden.DiscountPercent))/100;
            OrdenNueva.Total = (int)(restandoPorcentaje * nuevaOrden.Quantity);
            OrdenNueva.Status = "Pendiente";
            OrdenNueva.CartId = nuevaOrden.ClientId;
            OrdenNueva.ClientId = nuevaOrden.ClientId;
            var insert = TableOperation.Insert(OrdenNueva);
            var res = await table.ExecuteAsync(insert);
            var status = res.HttpStatusCode;

            var result = res.Result;
            if (res.Result != null)
            {
                ordersCounter++;
                Cart carrito = CartRepository.CartById(OrdenNueva.CartId);
                if(carrito == null)
                {
                    carrito.CartId = OrdenNueva.CartId;
                    carrito.CartTotal = OrdenNueva.Total;
                    MyOwnResponse createCartResponse = await CartRepository.CreateCart(carrito);
                    bool resultado = false;
                    if (createCartResponse.Result == "OK")
                    {
                         resultado = true;
                    } else
                    {
                        resultado = false;
                    }
                    if (resultado == true)
                    {
                        return new MyOwnResponse("OK", "Se agregó correctamente la orden y se creó el carrito para el cliente");
                    }
                    else
                    {
                        return new MyOwnResponse("OK", "Se agregó correctamente la orden pero tuvimos problemas para crear el carrito del cliente, inténtelo más tarde");
                    }
                } else
                {
                    return new MyOwnResponse("OK", "Se agrego la orden correctamente");
                }
            }
            else
            {
                return new MyOwnResponse("ERROR", "Hubo problemas al crear la orden.");
            }
        }

        public async Task<MyOwnResponse> UpdateOrder(Order orden)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Ordenes");
            TableOperation retrieveOperation = TableOperation.Retrieve<OrderEntity>("Ordenes", orden.OrderId.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                OrderEntity updateEntity = (OrderEntity)retrievedResult.Result;

                if(updateEntity.Status == "Pagada")
                {
                    return new MyOwnResponse("OK", "Lo sentimos, no podemos editar una orden que ya ha sido pagada.");
                }

                updateEntity.CartId = orden.CartId;
                updateEntity.ClientId = orden.ClientId;
                updateEntity.ProductId = orden.ProductId;
                updateEntity.Quantity = orden.Quantity;
                updateEntity.Status = orden.Status;
                updateEntity.Total = orden.Total;
                TableOperation updateOperation = TableOperation.Replace(updateEntity);
                var res = await table.ExecuteAsync(updateOperation);
                return new MyOwnResponse("OK", "La orden ha sido actualizada correctamente.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Lo sentimos, no podemos editar una orden que ya ha sido pagada.");
            }
        }

        public async Task<MyOwnResponse> DeleteOrder(Order orden)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Ordenes");
            TableOperation retrieveOperation = TableOperation.Retrieve<ProductoModelEntity>("Ordenes", orden.OrderId.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            OrderEntity deleteEntity = (OrderEntity)retrievedResult.Result;
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                var res = await table.ExecuteAsync(deleteOperation);
                MyOwnResponse response = new MyOwnResponse("OK","La orden se borró correctamente");
                return response;
            }
            else
            {
                MyOwnResponse response = new MyOwnResponse("ERROR", "Lo sentimos, no hemos encontrado registro de la orden.");
                return response;
            }
        }

        public List<Order> OrderByCart(int idCart)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Ordenes");

            TableQuery<OrderEntity> query = new TableQuery<OrderEntity>();
            List<Order> lista = new List<Order>();
            if (table.ExecuteQuery(query).Count() == 0)
            {
                return lista;
            }
            foreach (OrderEntity entity in table.ExecuteQuery(query))
            {
                if(entity.CartId == idCart) { 
                    Order orden = new Order()
                    {
                        OrderId = entity.OrderId,
                        CartId = entity.CartId,
                        ClientId = entity.ClientId,
                        ProductId = entity.ProductId,
                        Quantity = entity.Quantity,
                        Status = entity.Status,
                        Total = entity.Total
                    };
                    lista.Add(orden);
                }
            }
            return lista;
        }

        public int TotalCarrito(int idCart)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Ordenes");

            TableQuery<OrderEntity> query = new TableQuery<OrderEntity>();
            List<Order> lista = new List<Order>();
            int total = 0;
            if (table.ExecuteQuery(query).Count() == 0)
            {
                return total;
            }
            foreach (OrderEntity entity in table.ExecuteQuery(query))
            {
                if (entity.CartId == idCart)
                {
                    total = total + entity.Total;
                }
            }
            return total; 
        }

        public async Task<MyOwnResponse> PutIntoWishList(int idOrder)
        {
            Order wish = OrderById(idOrder);
            if(wish.OrderId == idOrder)
            {
                wish.CartId = 0;
                MyOwnResponse result = await UpdateOrder(wish);
                if(result != null)
                {
                    if(result.Result == "OK")
                    {
                        return new MyOwnResponse("OK", "La orden se ha añadido a la lista de deseos correctamente");
                    } else
                    {
                        return new MyOwnResponse("ERROR", "Hubo problema al añadir la orden a la lista de deseos");
                    }
                }
            }
            
            return new MyOwnResponse("ERROR","Problemas al buscar la Orden, verifica la información"); ;
        }

        public List<Order> MyWishList(int idClient)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Ordenes");

            TableQuery<OrderEntity> query = new TableQuery<OrderEntity>();
            List<Order> lista = new List<Order>();
            if (table.ExecuteQuery(query).Count() == 0)
            {
                return lista;
            }
            foreach (OrderEntity entity in table.ExecuteQuery(query))
            {
                if (entity.ClientId == idClient && entity.CartId == 0)
                {
                    Order orden = new Order()
                    {
                        OrderId = entity.OrderId,
                        CartId = entity.CartId,
                        ClientId = entity.ClientId,
                        ProductId = entity.ProductId,
                        Quantity = entity.Quantity,
                        Status = entity.Status,
                        Total = entity.Total,
                        ProductName = productRepository.ProductById(entity.ProductId).Name,
                        ClientName = ClientRepository.ClientById(entity.ClientId).Username                    
                    };
                    lista.Add(orden);
                }
            }
            return lista;
        }
    }
    
    public class OrderEntity : TableEntity
    {
        public OrderEntity(int orderId)
        {
            this.PartitionKey = "Ordenes";
            this.RowKey = orderId.ToString();
        }

        public OrderEntity() { }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public int CartId { get; set; }
        public int Total { get; set; }
        public int ClientId { get; set; }
        public string Status { get; set; }
    }
}