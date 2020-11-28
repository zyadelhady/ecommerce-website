using System.Threading.Tasks;
using e_commerce.DTOs;
using e_commerce.Entities;
using e_commerce.Helpers;

namespace e_commerce.Interfaces
{
  public interface IProductRepo
  {

    void AddProduct(Product product);
    Task<Product> GetProductByIdAsync(int id);
    Task<ProductDto> GetProductBySlugAsync(string slug);
    Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams);
    void DeleteProduct(Product product);
    void UpdateProduct(Product product);


  }
}