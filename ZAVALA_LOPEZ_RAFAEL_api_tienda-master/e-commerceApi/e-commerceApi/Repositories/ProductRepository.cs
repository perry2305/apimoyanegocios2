using e_commerceApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace e_commerceApi.Repositories
{
    interface ProductRepository
    {
        Task<MyOwnResponse> CreateProduct(Product p);
        List<Product> EveryProduct();
        Product ProductById(string Codigo);
        Task<MyOwnResponse> UpdateProduct(Product p);
        Task<MyOwnResponse> DeleteProduct(Product p);
        Task<Cart> AddToCart(int ProductId, int CartId, int Quantity);
    }

    public class ProductoModelEntity : TableEntity
    {
        public ProductoModelEntity(string Codigo)
        {
            this.PartitionKey = "Productos";
            this.RowKey = Codigo;
        }
        public ProductoModelEntity() { }
        public int Id { get; set; }
        public int Price { get; set; }
        public String Name { get; set; }
        public String ShortDescription { get; set; }
        public String LongDescription { get; set; }
        public int DiscountPercent { get; set; }
        public Boolean IsOffer { get; set; }
        public String PhotoUrl { get; set; }

    }

    public class MemoryProductRepository : ProductRepository
    {
        private static MemoryCartRepository carritos;
        private static int productsCounter = 0;
        private string ConnectionString;
        public MemoryProductRepository(string connectionString)
        {
            
            this.ConnectionString = Globals.connectionString;
            carritos = Globals.CartsRepository;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Productos");

            table.CreateIfNotExists();

            TableQuery<ProductoModelEntity> query = new TableQuery<ProductoModelEntity>();
            if (table.ExecuteQuery(query).Count() != 0)
            {
                List<ProductoModelEntity> ProductosEnTabla = table.ExecuteQuery(query).OrderBy(productEntity => productEntity.RowKey).ToList();
                int lastCartId = ProductosEnTabla.Last().Id;
                productsCounter = lastCartId + 1;
            }
        }


        public async Task<MyOwnResponse> CreateProduct(Product p)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Productos");

            table.CreateIfNotExists();

            var ProductoNuevo = new ProductoModelEntity(productsCounter.ToString());
            ProductoNuevo.Id = productsCounter;
            ProductoNuevo.Price = p.Price;
            ProductoNuevo.IsOffer = p.IsOffer;
            ProductoNuevo.ShortDescription = p.ShortDescription;
            ProductoNuevo.LongDescription = p.LongDescription;
            ProductoNuevo.Name = p.Name;
            ProductoNuevo.PhotoUrl = p.PhotoUrl;
            ProductoNuevo.DiscountPercent = p.DiscountPercent;
            var insert = TableOperation.Insert(ProductoNuevo);
            var res =  await table.ExecuteAsync(insert);

            if(res.Result != null)
            {
                productsCounter++;
                return new MyOwnResponse("OK","Se agregó el producto correctamente a la lista de productos.");
            } else
            {
                return new MyOwnResponse("ERROR","Problemas al agregar el producto.");
            }
        }
        
        public List<Product> EveryProduct()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Productos");

            TableQuery<ProductoModelEntity> query = new TableQuery<ProductoModelEntity>();
            List<Product> lista = new List<Product>();
            
            if (table.ExecuteQuery(query).Count() == 0)
            {
                return lista;
            }

            IEnumerable<ProductoModelEntity> productosTabla = table.ExecuteQuery(query);

            foreach (ProductoModelEntity entity in table.ExecuteQuery(query))
            {
                Product producto = new Product()
                {
                    Id = entity.Id,
                    DiscountPercent = entity.DiscountPercent,
                    Name = entity.Name,
                    Price = entity.Price,
                    IsOffer = entity.IsOffer,
                    ShortDescription = entity.ShortDescription,
                    LongDescription = entity.LongDescription,
                    PhotoUrl = entity.PhotoUrl
                };
                lista.Add(producto);
            }
            return lista;
        }
        public Product ProductById(int Id)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Productos");
            TableQuery<ProductoModelEntity> query = new TableQuery<ProductoModelEntity>();
           
            Product producto = new Product();
            foreach (ProductoModelEntity entity in table.ExecuteQuery(query))
            {
                if (Id == Int32.Parse(entity.RowKey))
                {
                    producto = new Product()
                    {
                        Id = entity.Id,
                        DiscountPercent =entity.DiscountPercent,
                        Name = entity.Name,
                        Price = entity.Price,
                        IsOffer = entity.IsOffer,
                        ShortDescription = entity.ShortDescription,
                        LongDescription = entity.LongDescription,
                        PhotoUrl = entity.PhotoUrl
                    };
                }

            }
            if (producto.Id.ToString() != null)
            {
                return producto;
            }
            else
            {
                return null;
            }
        }

        public async Task<MyOwnResponse> UpdateProduct(Product p)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Productos");
            TableOperation retrieveOperation = TableOperation.Retrieve<ProductoModelEntity>("Productos", p.Id.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if(retrievedResult.Result != null)
            {
                ProductoModelEntity updateEntity = (ProductoModelEntity)retrievedResult.Result;
                updateEntity.DiscountPercent = p.DiscountPercent;
                updateEntity.Id = p.Id;
                updateEntity.IsOffer = p.IsOffer;
                updateEntity.LongDescription = p.LongDescription;
                updateEntity.Name = p.Name;
                updateEntity.PhotoUrl = p.PhotoUrl;
                updateEntity.Price = p.Price;
                updateEntity.ShortDescription = p.ShortDescription;
                TableOperation updateOperation = TableOperation.Replace(updateEntity);
                var res = await table.ExecuteAsync(updateOperation);
                return new MyOwnResponse("OK","Se actualizó el producto correctamente.");
            }
            else
            {
                return new MyOwnResponse("ERROR","Problemas para actualizar el producto.");
            }
            
        }
        
        public async Task<MyOwnResponse> DeleteProduct(Product p)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Productos");
            TableOperation retrieveOperation = TableOperation.Retrieve<ProductoModelEntity>("Productos", p.Id.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            ProductoModelEntity deleteEntity = (ProductoModelEntity)retrievedResult.Result;
            if(deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                var res = await table.ExecuteAsync(deleteOperation);
                return new MyOwnResponse("OK","Se eliminó el producto correctamente.");
            } else
            {
                return new MyOwnResponse("ERROR","Problemas para eliminar el producto.");
            }
        }

        public async Task<string> UpdateImage(int productId, string fileName, Stream inputStream)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.ConnectionString);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference("imagenes");

            Product producto = ProductById(productsCounter - 1);

            //container.CreateIfNotExists();
            // Retrieve reference to a blob named "myblob".
            var name = producto.Id.ToString() + Path.GetExtension(fileName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(name);
            // Create or overwrite the "myblob" blob with contents from a local file.
            await blockBlob.UploadFromStreamAsync(inputStream);

            var urlPhoto =  blockBlob.SnapshotQualifiedUri.ToString();
            producto.PhotoUrl = urlPhoto;
            var res = await UpdateProduct(producto);
            if(res.Result == "OK")
            {
                return "OK";
            }else
            {
                return "ERROR";
            }
        }

        public Task<Cart> AddToCart(int ProductId, int CartId, int Quantity)
        {
            throw new NotImplementedException();
        }

        public Product ProductById(string Codigo)
        {
            throw new NotImplementedException();
        }
    }
}