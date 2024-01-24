using ProductMicroservice.Models;

namespace ProductMicroservice.Data.Interfaces
{
    public interface IProductRepo
    {
        IEnumerable<Product> GetAllProducts();
        Product EditProduct(Product product);
        Product GetProductById(int id);
        void DeleteProduct(Product product);
        void CreateProduct(Product product);
        Task<bool> SaveChangesAsync();
    }
}