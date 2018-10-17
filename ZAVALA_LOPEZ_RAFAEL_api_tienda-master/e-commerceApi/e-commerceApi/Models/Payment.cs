using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace e_commerceApi.Models
{
    public class Payment
    {
        [Key]
        public int PaymentId { get; set; }
        public int ClientId { get; set; }
        public string ClientName { get; set; }
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public string Reference { get; set; }
        public string PaymentDate { get; set; }
        public bool Status { get; set; }
    }
    public class PaymentEntity : TableEntity
    {
        public PaymentEntity(int idPago)
        {
            this.RowKey = idPago.ToString();
            this.PartitionKey = "Pagos";
        }
        public PaymentEntity() { }
        public int PaymentId { get; set; }
        public int ClientId { get; set; }
        public int OrderId { get; set; }
        public int Amount { get; set; }
        public string Reference { get; set; }
        public string PaymentDate { get; set; }
        public bool Status { get; set; }
    }
}