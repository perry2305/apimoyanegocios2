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
    interface ClientRepository
    {
        List<Client> EveryClient();
        Client ClientById(int id);
        Task<MyOwnResponse> CreateClient(Client cliente);
        Task<MyOwnResponse> UpdateClient(Client cliente);
        Task<MyOwnResponse> DeleteClient(Client cliente);
        MyOwnResponse LoginClient(string username, string MD5password);
    }
    public class ClientInfoEntity: TableEntity
    {
        public ClientInfoEntity(int infoId)
        {
            this.PartitionKey = "InformacionClientes";
            this.RowKey = infoId.ToString();
        }
        public ClientInfoEntity() { }
        public int InfoId { get; set; }
        public string Name { get; set; }
        public string BirthDate { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Password { get; set; }
        public int ClientId { get; set; }
        public string Username { get; set; }
    }

    public class ClientEntity : TableEntity
    {
        public ClientEntity(int ClientId)
        {
            this.PartitionKey = "Clientes";
            this.RowKey = ClientId.ToString();
        }
        public ClientEntity() { }
        public int ClientId { get; set; }
        public String Username { get; set; }
        public String Password { get; set; }
        public int CarritoId { get; set; }
        public string UserType { get; set; }
        public int InfoId { get; set; }
    }

    public class MemoryClientRepository : ClientRepository
    {
        private static int clientsCounter = 1;
        private static int infoClientsCounter = 1;
        private string ConnectionString;
        public MemoryClientRepository(string connectionString)
        {
            ConnectionString = connectionString;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Clientes");

            table.CreateIfNotExists();
            if (table.Exists())
            {
                TableQuery<ClientEntity> query = new TableQuery<ClientEntity>();
                if (table.ExecuteQuery(query).Count() != 0)
                {
                    List<ClientEntity> EnTabla = table.ExecuteQuery(query).OrderByDescending(cliente => cliente.RowKey).ToList();
                    int lastClientId = int.Parse(EnTabla.First().RowKey);
                    clientsCounter = lastClientId + 1;
                }
            }
            
            table = tableClient.GetTableReference("InformacionClientes");
            if (table.Exists())
            {
                TableQuery<ClientInfoEntity> query = new TableQuery<ClientInfoEntity>();
                if (table.ExecuteQuery(query).Count() != 0)
                {
                    List<ClientInfoEntity> EnTabla = table.ExecuteQuery(query).OrderByDescending(cliente => cliente.Timestamp).ToList();
                    int lastClientInfoId = int.Parse(EnTabla.First().RowKey);
                    infoClientsCounter = lastClientInfoId + 1;
                }
            }
        }

        public Client ClientById(int id)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Clientes");
            TableOperation retrieveOperation = TableOperation.Retrieve<ClientEntity>("Clientes",id.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                ClientEntity clienteEntityEncontrado = (ClientEntity)retrievedResult.Result;
                Client clienteEncontrado = new Client()
                {
                    ClientId = clienteEntityEncontrado.ClientId,
                    CarritoId = clienteEntityEncontrado.CarritoId,
                    Password = clienteEntityEncontrado.Password,
                    Username = clienteEntityEncontrado.Username,
                    UserType = clienteEntityEncontrado.UserType
                };
                return clienteEncontrado;
            }
            return null;
        }

        public async Task<MyOwnResponse> CreateClient(Client cliente)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Clientes");

            table.CreateIfNotExists();

            var ClienteNuevo = new ClientEntity(clientsCounter);
            ClienteNuevo.CarritoId = cliente.CarritoId;
            ClienteNuevo.Password = cliente.Password;
            ClienteNuevo.Username = cliente.Username;
            ClienteNuevo.UserType = cliente.UserType;
            ClienteNuevo.ClientId = clientsCounter;
            ClienteNuevo.InfoId = cliente.InfoId;

            var insert = TableOperation.Insert(ClienteNuevo);
            var res = table.Execute(insert);
            var result = res.Result;
            if (res.Result != null)
            {
                clientsCounter++;
                return new MyOwnResponse("OK", "Se agregó el cliente correctamente a la lista de clientes.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Problemas al agregar al cliente.");
            }
        }

        public async Task<MyOwnResponse> DeleteClient(Client cliente)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Clientes");
            TableOperation retrieveOperation = TableOperation.Retrieve<ClientEntity>("Clientes", cliente.ClientId.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            ClientEntity deleteEntity = (ClientEntity)retrievedResult.Result;
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                var res = await table.ExecuteAsync(deleteOperation);
                return new MyOwnResponse("OK", "Se eliminó el cliente correctamente.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Problemas para eliminar el cliente.");
            }
        }

        public List<Client> EveryClient()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Clientes");

            TableQuery<ClientEntity> query = new TableQuery<ClientEntity>();
            List<Client> lista = new List<Client>();

            if (table.ExecuteQuery(query).Count() == 0)
            {
                return lista;
            }

            IEnumerable<ClientEntity> productosTabla = table.ExecuteQuery(query);

            foreach (ClientEntity entity in table.ExecuteQuery(query))
            {
                Client cliente = new Client()
                {
                    CarritoId = entity.CarritoId,
                    ClientId = entity.ClientId,
                    Password = entity.Password,
                    Username = entity.Username,
                    UserType = entity.UserType,
                    InfoId = entity.InfoId
                };
                lista.Add(cliente);
            }
            return lista;
        }

        public async Task<MyOwnResponse> UpdateInformation(ClientInfoEntity infoEntity)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("InformacionClientes");
            TableOperation updateOperation = TableOperation.Replace(infoEntity);
            var res = await table.ExecuteAsync(updateOperation);
            return new MyOwnResponse("OK", "Se actualizó el cliente correctamente.");
        }

        public async Task<MyOwnResponse> AcceptRegister(int infoId)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("InformacionClientes");
            TableOperation retrieveOperation = TableOperation.Retrieve<ClientInfoEntity>("InformacionClientes", infoId.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                ClientInfoEntity updateEntity = (ClientInfoEntity)retrievedResult.Result;
                updateEntity.ClientId = clientsCounter;
                Client cliente = new Client()
                {
                    Username = updateEntity.Username,
                    Password = updateEntity.Password,
                    InfoId = infoId,
                    ClientId = clientsCounter,
                    CarritoId = 0,
                    UserType = "CLIENTE"
                };
                var res = await CreateClient(cliente);
                TableOperation updateOperation = TableOperation.Replace(updateEntity);
                //CloudTable table2 = tableClient.GetTableReference("Clientes");
                table.Execute(updateOperation);
                clientsCounter++;

                return new MyOwnResponse("OK", "Se acepto el registro del cliente correctamente.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Hubo un problema al aceptar el registro, intentelo de nuevo más tarde");
            }
        }

        public List<ClientInfo> ShowRegisters()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("InformacionClientes");

            table.CreateIfNotExists();
            TableQuery<ClientInfoEntity> query = new TableQuery<ClientInfoEntity>();
            List<ClientInfo> lista = new List<ClientInfo>();

            if (table.ExecuteQuery(query).Count() == 0)
            {
                return lista;
            }

            IEnumerable<ClientInfoEntity> productosTabla = table.ExecuteQuery(query);

            foreach (ClientInfoEntity entity in table.ExecuteQuery(query))
            {
                if (entity.ClientId == 0)
                {
                    ClientInfo informacion = new ClientInfo()
                    {
                        Address = entity.Address,
                        BirthDate = entity.BirthDate,
                        Email = entity.Email,
                        ClientId = 0,
                        InfoId = entity.InfoId,
                        Name = entity.Name,
                        Password = entity.Password,
                        Username = entity.Username
                    };
                    lista.Add(informacion);
                }
            }
            return lista;
        }

        public async Task<MyOwnResponse> SignUpClient(ClientInfo information)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("InformacionClientes");

            table.CreateIfNotExists();

            var ClientInfoNueva = new ClientInfoEntity(infoClientsCounter);
            ClientInfoNueva.Address = information.Address;
            ClientInfoNueva.BirthDate = information.BirthDate;
            ClientInfoNueva.ClientId = 0;
            ClientInfoNueva.Email = information.Email;
            ClientInfoNueva.InfoId = infoClientsCounter;
            ClientInfoNueva.Name = information.Name;
            ClientInfoNueva.Password = information.Password;
            ClientInfoNueva.Username = information.Username;

            var insert = TableOperation.Insert(ClientInfoNueva);
            var res = await table.ExecuteAsync(insert);

            if (res.Result != null)
            {
                infoClientsCounter++;
                return new MyOwnResponse("OK", "Se añadió a la lista de solicitudes de registro, un administrador podrá darle acceso a la tienda");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Hubó problemas al crear la solicitud de registro, intentélo más tarde.");
            }
        }

        public MyOwnResponse LoginClient(string username, string MD5password)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Clientes");

            TableQuery<ClientEntity> query = new TableQuery<ClientEntity>()
                .Where(TableQuery.GenerateFilterCondition("Username", QueryComparisons.Equal, username));
            if (table.ExecuteQuery(query).Count() == 0)
            {
                return new MyOwnResponse("ERROR", "El usuario no existe");
            }
            foreach (ClientEntity entity in table.ExecuteQuery(query))
            {
                if (MD5password == entity.Password && entity.UserType == "CLIENTE")
                {
                    return new MyOwnResponse("OK", entity.ClientId.ToString());
                }
                else
                {
                    return new MyOwnResponse("ERROR", "El usuario y/o contraseña son incorrectos");
                }
            }
            return new MyOwnResponse("ERROR", "Error al iniciar sesion, no se encontro la información del usuario");
        }

        public async Task<MyOwnResponse> UpdateClient(Client cliente)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Clientes");
            TableOperation retrieveOperation = TableOperation.Retrieve<ProductoModelEntity>("Clientes", cliente.ClientId.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                ClientEntity updateEntity = (ClientEntity)retrievedResult.Result;
                updateEntity.CarritoId = cliente.CarritoId;
                updateEntity.ClientId = cliente.ClientId;
                updateEntity.Password = cliente.Password;
                updateEntity.Username = cliente.Username;
                updateEntity.UserType = cliente.UserType;
                TableOperation updateOperation = TableOperation.Replace(updateEntity);
                var res = await table.ExecuteAsync(updateOperation);
                return new MyOwnResponse("OK", "Se actualizó el cliente correctamente.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Problemas para actualizar el cliente.");
            }
        }
    }
}
