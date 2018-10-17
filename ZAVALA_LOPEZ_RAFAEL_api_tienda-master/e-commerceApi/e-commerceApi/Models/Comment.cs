using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace e_commerceApi.Models
{
    public class Comment
    {
        [Key]
        public int CommentId { get; set; }
        public int ProductId { get; set; }
        public string ClientName { get; set; }
        public DateTime PublishDate { get; set; }
        public string CommentBody { get; set; }
        //public int Ranked { get; set; }   Opcional por si ponemos estrellas!
    }
}