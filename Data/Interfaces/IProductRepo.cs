using ProductMicroservice.Models;

namespace ProductMicroservice.Data.Interfaces
{
    public interface IProductRepo
    {
        void CreateProduct(Product product);
        Task<bool> SaveChangesAsync();
    }
}