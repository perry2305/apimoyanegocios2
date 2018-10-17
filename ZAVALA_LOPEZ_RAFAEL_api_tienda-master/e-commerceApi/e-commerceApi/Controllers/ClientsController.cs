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
using WhatsAppApi;

namespace e_commerceApi.Controllers
{
    public class ClientsController : ApiController
    {
        private e_commerceApiContext db = new e_commerceApiContext();
        private MemoryClientRepository clientes;

        public ClientsController()
        {
            clientes = Globals.ClientRepository;
        }
        // GET: api/Clients
        public IEnumerable<Client> GetClients()
        {
            if(clientes.EveryClient().Count() > 0)
            {
                return clientes.EveryClient();
            } else
            {
                return Enumerable.Empty<Client>();
            }
        }

        // GET: api/Clients/5
        
        public Client GetClient(int id)
        {
            Client cliente = clientes.ClientById(id);
            if (cliente != null)
            {
                return clientes.ClientById(id);
            }
            else
            {
                return null;
            }
        }

        // PUT: api/Clients/5
        [ResponseType(typeof(void))]
        public async Task<MyOwnResponse> Put(int id, Client client)
        {
            return await clientes.UpdateClient(client);
        }

        // POST: api/Clients
        public async Task<MyOwnResponse> Post(Client client)
        {
            return await clientes.CreateClient(client);
        }

        // DELETE: api/Clients/5
        public async Task<MyOwnResponse> DeleteClient(int id)
        {
            return await clientes.DeleteClient(clientes.ClientById(id));
        }

        [Route("api/LoginClient/{username}/{password}")]
        [HttpPost]
        public MyOwnResponse LoginClient(string username, string password)
        {
            return clientes.LoginClient(username, password);
        }
        [Route("api/SignUpClient")]
        [HttpPost]
        public async Task<MyOwnResponse> SignUpClient(ClientInfo information)
        {
            return await clientes.SignUpClient(information);
        }

        [Route("api/AcceptRegister/{infoId}")]
        [HttpPost]
        public async Task<MyOwnResponse> AcceptRegister(int infoId)
        {
            //SendMessage("6673233813", "Hola wey desde la api");
            return await clientes.AcceptRegister(infoId);
        }

        [Route("api/Registros")]
        [HttpGet]
        public List<ClientInfo> ShowRegisters()
        {
            return clientes.ShowRegisters();
        }


        public string SendMessage(string To, string Message)
        {
            string status = "";
            WhatsApp wa = new WhatsApp("6673233813", "12343210", "Rafael", false, false);
            wa.OnConnectSuccess += () =>
            {
                wa.OnLoginSuccess += (phoneNumber, data) =>
                 {
                     status = "Connection Success";
                     wa.SendMessage(To, Message);
                     status = "Mensaje enviado";
                 };
                wa.OnLoginFailed += (data) =>
                {
                    status = "Login failed" + data;
                };
                wa.Login();
            };
            wa.OnConnectFailed += (ex) =>
            {
                status = "Connection Failed " + ex.StackTrace;
            };
           wa.Connect();
            return status;
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