using e_commerceApi.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace e_commerceApi.Repositories
{
    interface CategoryRepository
    {
        Category CategorybyId(int id);
        List<Category> EveryCategory();
        Task<MyOwnResponse> CreateCategory(Category categoria);
        Task<MyOwnResponse> UpdateCategory(Category categoria);
        Task<MyOwnResponse> DeleteCategory(Category categoria);
        Task<MyOwnResponse> AddProductToCategory(int producto, int categoria);
        List<Product> ProductByCategory(int idCategoria);
    }
    public class MemoryCategoryRepository : CategoryRepository
    {
        public string ConnectionString;
        public int categoriesCounter = 0;
        public int categoriaProductoCounter = 0;
        public MemoryProductRepository ProductsRepository;

        public MemoryCategoryRepository(string connection)
        {
            ConnectionString = connection;
            ProductsRepository = Globals.ProductsRepository;

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Categorias");

            table.CreateIfNotExists();

            TableQuery<CategoryEntity> query = new TableQuery<CategoryEntity>();
            if (table.ExecuteQuery(query).Count() != 0)
            {
                List<CategoryEntity> EnTabla = table.ExecuteQuery(query).OrderBy(category => category.RowKey).ToList();
                int lastCartId = int.Parse(EnTabla.Last().RowKey);
                categoriesCounter = lastCartId + 1;
            }
            CloudTable table2 = tableClient.GetTableReference("CategoriaProducto");

            table2.CreateIfNotExists();

            TableQuery<CategoryProductEntity> query2 = new TableQuery<CategoryProductEntity>();
            if (table2.ExecuteQuery(query).Count() != 0)
            {
                List<CategoryProductEntity> EnTabla = table2.ExecuteQuery(query2).OrderBy(category => category.RowKey).ToList();
                int lastCartId = int.Parse(EnTabla.Last().RowKey);
                categoriaProductoCounter = lastCartId + 1;
            }
        }
        public Category CategorybyId(int id)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Categorias");
            TableOperation retrieveOperation = TableOperation.Retrieve<CategoryEntity>("Categorias", id.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                CategoryEntity entity = (CategoryEntity)retrievedResult.Result;
                Category categoria = new Category()
                {
                    CategoryId = entity.CategoryId,
                    CategoryName = entity.CategoryName,
                    Description = entity.Description
                };
                return categoria;
            }
            else
            {
                return null;
            }
        }

        public async Task<MyOwnResponse> CreateCategory(Category categoria)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Categorias");

            table.CreateIfNotExists();
            CategoryEntity Nueva = new CategoryEntity(categoriesCounter);
            Nueva.CategoryId = categoriesCounter;
            Nueva.CategoryName = categoria.CategoryName;
            Nueva.Description = categoria.Description;
            Nueva.Products = 0;

            var insert = TableOperation.Insert(Nueva);
            var res = await table.ExecuteAsync(insert);

            if (res.Result != null)
            {
                categoriesCounter++;
                return new MyOwnResponse("OK", "Se agregó el producto correctamente a la lista de productos.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Problemas al agregar el producto.");
            }
        }

        public List<Category> EveryCategory()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Categorias");

            TableQuery<CategoryEntity> query = new TableQuery<CategoryEntity>();
            List<Category> lista = new List<Category>();

            if (table.ExecuteQuery(query).Count() == 0)
            {
                return lista;
            }

            IEnumerable<CategoryEntity> categoriasTabla = table.ExecuteQuery(query);

            foreach (CategoryEntity entity in table.ExecuteQuery(query))
            {
                Category categoria = new Category()
                {
                    CategoryId = entity.CategoryId,
                    CategoryName = entity.CategoryName,
                    Description = entity.Description,
                    Products = entity.Products
                };
                lista.Add(categoria);
            }
            return lista;
        }

        public async Task<MyOwnResponse> DeleteCategory(Category categoria)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Categorias");
            TableOperation retrieveOperation = TableOperation.Retrieve<CategoryEntity>("Categorias", categoria.CategoryId.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            CategoryEntity deleteEntity = (CategoryEntity)retrievedResult.Result;
            if (deleteEntity != null)
            {
                TableOperation deleteOperation = TableOperation.Delete(deleteEntity);
                var res = await table.ExecuteAsync(deleteOperation);
                return new MyOwnResponse("OK", "Se eliminó el producto correctamente.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Problemas para eliminar el producto.");
            }
        }

        public async Task<MyOwnResponse> UpdateCategory(Category categoria)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("Categorias");
            TableOperation retrieveOperation = TableOperation.Retrieve<CategoryEntity>("Categorias", categoria.CategoryId.ToString());
            TableResult retrievedResult = table.Execute(retrieveOperation);

            if (retrievedResult.Result != null)
            {
                CategoryEntity updateEntity = (CategoryEntity)retrievedResult.Result;
                updateEntity.CategoryId = categoria.CategoryId;
                updateEntity.CategoryName = categoria.CategoryName;
                updateEntity.Description = categoria.Description;
                updateEntity.Products = categoria.Products;
                TableOperation updateOperation = TableOperation.Replace(updateEntity);
                var res = await table.ExecuteAsync(updateOperation);
                return new MyOwnResponse("OK", "Se actualizó el producto correctamente.");
            }
            else
            {
                return new MyOwnResponse("ERROR", "Problemas para actualizar el producto.");
            }

        }

        public async Task<MyOwnResponse> AddProductToCategory(int idProducto, int idCategoria)
        {
            Category categoria = CategorybyId(idCategoria);
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("CategoriaProducto");

            table.CreateIfNotExists();
            CategoryProductEntity Nueva = new CategoryProductEntity(categoriaProductoCounter);
            Nueva.CategoryId = idCategoria;
            Nueva.ProductId = idProducto;
           
            var insert = TableOperation.Insert(Nueva);
            var res = await table.ExecuteAsync(insert);

            if (res.Result != null)
            {
                categoriaProductoCounter++;
                categoria.Products = categoria.Products + 1;
                var res2 = await UpdateCategory(categoria);
                if(res2.Result != null)
                {
                    return new MyOwnResponse("OK", "Se agregó el producto a la categoria.");
                }
                else
                {
                    TableOperation deleteOperation = TableOperation.Delete(Nueva);
                    var rollback = await table.ExecuteAsync(deleteOperation);
                    if(rollback.Result != null)
                    {
                        return new MyOwnResponse("ERROR", "No se pudo agregar a las categorias, pero puedes intentarlo de nuevo, eliminamos el registro");
                    }else
                    {
                        return new MyOwnResponse("ERROR", "Hubo un problema en el rollback, consulta a un administrador");
                    }
                }
            }
            else
            {
                return new MyOwnResponse("ERROR", "Problemas al agregar el producto.");
            }
        }

        public List<Product> ProductByCategory(int idCategoria)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("CategoriaProducto");

            TableQuery<CategoryProductEntity> query = new TableQuery<CategoryProductEntity>();
            List<Product> lista = new List<Product>();
            if (table.ExecuteQuery(query).Count() == 0)
            {
                return lista;
            }
            foreach (CategoryProductEntity entity in table.ExecuteQuery(query))
            {
                if(entity.CategoryId == idCategoria)
                {
                    lista.Add(ProductsRepository.ProductById(entity.ProductId));
                }
            }
            return lista;
        }

        public List<Category> CategoriesByProduct(int idProducto)
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionString);
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            CloudTable table = tableClient.GetTableReference("CategoriaProducto");

            TableQuery<CategoryProductEntity> query = new TableQuery<CategoryProductEntity>();
            List<Category> lista = new List<Category>();
            if (table.ExecuteQuery(query).Count() == 0)
            {
                return lista;
            }
            foreach (CategoryProductEntity entity in table.ExecuteQuery(query))
            {
                if (entity.ProductId == idProducto)
                {
                    lista.Add(CategorybyId(entity.CategoryId));
                }
            }
            return lista;
        }
    }
}