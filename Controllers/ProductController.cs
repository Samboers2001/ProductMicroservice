using AccountMicroservice.AsyncDataServices.Interfaces;
using Microsoft.AspNetCore.Mvc;
using ProductMicroservice.Data.Interfaces;
using ProductMicroservice.MessageBusEvents;
using ProductMicroservice.Models;

namespace ProductMicroservice.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepo _repository;
        private readonly IMessageBusClient _messageBus;

        public ProductController(IProductRepo repository, IMessageBusClient messageBus)
        {
            _repository = repository;
            _messageBus = messageBus;
        }

        [HttpPost]
        [Route("create")]
        public ActionResult<Product> CreateProduct(Product model)
        {
            Product product = new Product
            {
                Name = model.Name,
                Price = model.Price,
                Description = model.Description,
                Created = DateTime.UtcNow
            };
            _repository.CreateProduct(product);
            _repository.SaveChangesAsync();
            ProductCreatedEvent productCreatedEvent = new ProductCreatedEvent
            {
                ExternalProductId = product.Id
            };
            Console.WriteLine(productCreatedEvent.ExternalProductId);
            _messageBus.PublishMessage(productCreatedEvent, "product.created");
            return Ok(new { message = "Product created successfully", product });
        }
    }
}