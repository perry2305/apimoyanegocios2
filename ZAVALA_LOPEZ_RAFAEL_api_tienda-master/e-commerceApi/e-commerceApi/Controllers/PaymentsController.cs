using e_commerceApi.Models;
using e_commerceApi.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace e_commerceApi.Controllers
{
    public class PaymentsController : ApiController
    {
        private MemoryPaymentRepository pagos;


        public PaymentsController()
        {
            pagos = Globals.PaymentRepository;
        }
        // GET: api/Payments
        public IEnumerable<Payment> Get()
        {
            return pagos.EveryPayment();
        }

        // GET: api/Payments/5
        public Payment Get(int id)
        {
            return pagos.PaymentById(id);
        }

        // POST: api/Payments
        public async Task<MyOwnResponse> Post([FromBody]Payment pago)
        {
            return await pagos.CreatePayment(pago);
        }

        // PUT: api/Payments/5
        public async Task<MyOwnResponse> Put([FromBody]Payment pago)
        {
            return await pagos.UpdatePayment(pago);
        }

        // DELETE: api/Payments/5
        public async Task<MyOwnResponse> Delete(int id)
        {
            return await pagos.DeletePayment(id);
        }

        [Route("api/Payments/{idPayment}/verify")]
        [HttpPost]

        public async Task<MyOwnResponse> verifyPayment(int idPayment)
        {
           return await pagos.VerifyPayment(idPayment);
        }
    }
}
