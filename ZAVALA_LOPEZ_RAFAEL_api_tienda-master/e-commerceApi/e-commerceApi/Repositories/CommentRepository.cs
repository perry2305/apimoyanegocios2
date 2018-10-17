using e_commerceApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace e_commerceApi.Repositories
{
    interface CommentRepository
    {
        Comment CommentById(int id);
        List<Comment> EveryComment();
        List<Comment> CommentByProduct(int idProducto);
        void CreateComment(Comment comentario);
        void DeleteComment(Comment comentario);
        void UpdateComment(Comment comentario);
    }

    public class MemoryCommentRepository : CommentRepository
    {
        private static List<Comment> comentarios;
        static MemoryCommentRepository()
        {
            comentarios = new List<Comment>();

            Comment comentario1 = new Models.Comment()
            {
                CommentId = 1,
                ProductId = 1,
                ClientName = "Rafael",
                CommentBody = "Muy buen producto, recomendadisimo!",
                PublishDate = new DateTime(2017, 05, 26)
            };
            Comment comentario2 = new Models.Comment()
            {
                CommentId = 2,
                ProductId = 1,
                ClientName = "Miguel",
                CommentBody = "Muy buen producto!",
                PublishDate = new DateTime(2017, 05, 26)
            };
            Comment comentario3 = new Models.Comment()
            {
                CommentId = 3,
                ProductId = 1,
                ClientName = "Marco",
                CommentBody = "Muy buen producto, recomendadisimo!",
                PublishDate = new DateTime(2017, 05, 26)
            };
            comentarios.Add(comentario1);
            comentarios.Add(comentario2);
            comentarios.Add(comentario3);
        }

        public Comment CommentById(int id)
        {
            return comentarios.FirstOrDefault(c => c.CommentId == id);
        }

        public List<Comment> CommentByProduct(int idProducto)
        {
            List<Comment> ComentariosDelProducto = new List<Comment>();
            foreach (Comment c in comentarios)
            {
                if(c.ProductId == idProducto)
                {
                    ComentariosDelProducto.Add(c);
                }
            }
            return ComentariosDelProducto;
        }

        public void CreateComment(Comment comentario)
        {
            comentarios.Add(comentario);
        }

        public void DeleteComment(Comment comentario)
        {
            comentarios.Remove(comentario);
        }

        public List<Comment> EveryComment()
        {
            return comentarios.ToList();
        }

        public void UpdateComment(Comment comentario)
        {
            Comment old = CommentById(comentario.CommentId);
            comentarios.Remove(old);
            comentarios.Add(comentario);
        }
    }
}
