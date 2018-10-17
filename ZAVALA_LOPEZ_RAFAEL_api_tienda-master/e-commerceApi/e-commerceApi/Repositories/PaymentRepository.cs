using e_commerceApi.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerceApi.Repositories
{
    interface PaymentRepository
    {
        List<Payment> EveryPayment();
        Payment PaymentById(int id);
        Task<MyOwnResponse> CreatePayment(Payment pago);
        Task<MyOwnResponse> UpdatePayment(Payment pago);
        Task<MyOwnResponse> DeletePayment(int id);
        List<Payment> PaymentsByClient(int clientId);
        Task<MyOwnResponse> VerifyPayment(int idpago);
    }

    public class MemoryPaymentRepository : PaymentRepository
    {
        private static List<Payment> pagos;
        public int PaymentCounter = 0;
        public string ConnectionString;
        private static MemoryClientRepository ClientRepository;
        private static MemoryCartRepository CartRepository;
        private static MemoryOrderRepository OrderRepository;


        public MemoryPaymentRepository(string connection)
        {
            ConnectionString = Globals.connectionString;
            ClientRepository = Globals.ClientRepository;
            CartRepository = Globals.CartsRepository;
            OrderRepository = Globals.OrdersRepository;
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Pagos");

            table.CreateIfNotExists();

            TableQuery<PaymentEntity> query = new TableQuery<PaymentEntity>();
            if (table.ExecuteQuery(query).Count() != 0)
            {
                List<PaymentEntity> EnTabla = table.ExecuteQuery(query).OrderBy(pago => pago.RowKey).ToList();
                int lastPaymentId = int.Parse(EnTabla.Last().RowKey);
                PaymentCounter = lastPaymentId + 1;
            }
            else
            {
                PaymentCounter = 1;
            }
        }

        public async Task<MyOwnResponse> DeletePayment(int id)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Pagos");
            TableOperation retrieveOperation = TableOperation.Retrieve<PaymentEntity>("Pagos", id.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            PaymentEntity deleteEntity = (PaymentEntity)retrievedResult.Result;
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                var res = await table.ExecuteAsync(deleteOperation);
                return new MyOwnResponse("OK", "Se eliminó el pago correctamente.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Problemas para eliminar el producto.");
            }
        }

        public List<Payment> EveryPayment()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Pagos");

            TableQuery<PaymentEntity> query = new TableQuery<PaymentEntity>();
            List<Payment> lista = new List<Payment>();

            if (table.ExecuteQuery(query).Count() == 0)
            {
                return lista;
            }

            IEnumerable<PaymentEntity> categoriasTabla = table.ExecuteQuery(query);

            foreach (PaymentEntity entity in table.ExecuteQuery(query))
            {
                Payment pago = new Payment()
                {
                    ClientId = entity.ClientId,
                    Amount = entity.Amount,
                    OrderId = entity.OrderId,
                    PaymentId = entity.PaymentId,
                    PaymentDate = entity.PaymentDate,
                    Reference = entity.Reference,
                    Status = entity.Status,
                    ClientName = ClientRepository.ClientById(entity.ClientId).Username
                };
                lista.Add(pago);
            }
            lista.OrderBy(ord => ord.PaymentId).ToList();
            return lista;
        }

        public Payment PaymentById(int id)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Pagos");
            TableOperation retrieveOperation = TableOperation.Retrieve<PaymentEntity>("Pagos", id.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                PaymentEntity entity = (PaymentEntity)retrievedResult.Result;
                Payment pago = new Payment()
                {
                    Amount = entity.Amount,
                    ClientId = entity.ClientId,
                    OrderId = entity.OrderId,
                    PaymentDate = entity.PaymentDate,
                    PaymentId = entity.PaymentId,
                    Reference = entity.Reference,
                    Status = entity.Status
                };
                return pago;
            }
            else
            {
                return null;
            }
        }

        public List<Payment> PaymentsByClient(int clientId)
        {
           /* Client cliente = clientes.ClientById(clientId);
            List<Payment> pagosPorCliente = new List<Payment>();
            foreach(Payment pago in pagos)
            {
                if(pago.ClientId == clientId)
                {
                    pagosPorCliente.Add(pago);
                }
            }
            return pagosPorCliente;   */
            return pagos.Where(x => x.ClientId == clientId).ToList();
        }

        public async Task<MyOwnResponse> VerifyPayment(int paymentId)
        {
            Payment pago = PaymentById(paymentId);
            pago.Status = true;
            var res = await UpdatePayment(pago);
            if(res.Result == "OK")
            {

                Order orden = OrderRepository.OrderById(pago.OrderId);
                orden.Status = "Pagada";
                var res2 = await OrderRepository.UpdateOrder(orden);
                if(res2.Result == "OK")
                {
                    return new MyOwnResponse("OK", "Se verifico el pago correctamente");
                }else
                {
                    var res3 = await DeletePayment(pago.PaymentId);
                    if(res3.Result == "OK")
                    {
                        return new MyOwnResponse("ERROR", "Hubo errores, pero se hizo un rollback correcto");
                    }
                    else
                    {
                        return new MyOwnResponse("ERROR", "Hubo errores, y tambien fallo el rollback, consulta a tu administrador");
                    }
                }
            }
            else
            {
                return new MyOwnResponse("ERROR", "Error actualizar el pago");
            }

        }


        public async Task<MyOwnResponse> UpdatePayment(Payment pago)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Pagos");
            TableOperation retrieveOperation = TableOperation.Retrieve<PaymentEntity>("Pagos", pago.PaymentId.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                PaymentEntity updateEntity = (PaymentEntity)retrievedResult.Result;
                updateEntity.Amount = pago.Amount;
                updateEntity.ClientId = pago.ClientId;
                updateEntity.OrderId = pago.OrderId;
                updateEntity.PaymentDate = pago.PaymentDate;
                updateEntity.Reference = pago.Reference;
                updateEntity.Status = pago.Status;
                TableOperation updateOperation = TableOperation.Replace(updateEntity);
                var res = await table.ExecuteAsync(updateOperation);
                return new MyOwnResponse("OK", "Se actualizó el pago correctamente.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Problemas para actualizar el pago.");
            }
        }

        public async Task<MyOwnResponse> CreatePayment(Payment pago)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Pagos");

            table.CreateIfNotExists();
            PaymentEntity Nuevo = new PaymentEntity(PaymentCounter);
            Nuevo.Amount = pago.Amount;
            Nuevo.ClientId = pago.ClientId;
            Nuevo.OrderId = pago.OrderId;
            Nuevo.PaymentDate = pago.PaymentDate;
            Nuevo.PaymentId = PaymentCounter;
            Nuevo.Reference = pago.Reference;
            Nuevo.Status = false;

            
            var insert = TableOperation.Insert(Nuevo);
            var res = await table.ExecuteAsync(insert);

            if (res.Result != null)
            {
                Order orden = OrderRepository.OrderById(pago.OrderId);
                orden.Status = "Pendiente";
                var res2 = await OrderRepository.UpdateOrder(orden);
                PaymentCounter++;
                return new MyOwnResponse("OK", "Se registró el pago correctamente.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Problemas al registrar el pago.");
            }
        }
    }
}
