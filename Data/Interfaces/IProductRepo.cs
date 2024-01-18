using ProductMicroservice.Models;

namespace ProductMicroservice.Data.Interfaces
{
    public interface IProductRepo
    {
        IEnumerable<Product> GetAllProducts();
        void CreateProduct(Product product);
        Task<bool> SaveChangesAsync();
    }
}