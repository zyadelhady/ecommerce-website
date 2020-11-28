using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using e_commerce.Data;
using e_commerce.DTOs;
using e_commerce.Entities;
using e_commerce.Helpers;
using e_commerce.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Repositories
{
  public class ProductRepo : IProductRepo
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public ProductRepo(DataContext context, IMapper mapper)
    {
      _mapper = mapper;
      _context = context;
    }

    public void AddProduct(Product product)
    {
      _context.Products.Add(product);
    }

    public void DeleteProduct(Product product)
    {
      _context.Products.Remove(product);
    }

    public async Task<Product> GetProductByIdAsync(string id)
    {
      return await _context.Products.FindAsync(id);
    }

    public async Task<ProductDto> GetProductBySlugAsync(string slug)
    {
      return await _context.Products
        .Where(p => p.Slug == slug)
        .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
        .FirstOrDefaultAsync();
    }

    public async Task<PagedList<ProductDto>> GetProductsAsync(ProductParams productParams)
    {
      var query = _context.Products.ProjectTo<ProductDto>(_mapper.ConfigurationProvider);

      query = string.IsNullOrEmpty(productParams.CollectionName) ? query : query.Where(p => p.CollectionName == productParams.CollectionName);

      query = productParams.Price switch
      {
        "high" => query.OrderByDescending(p => (double)p.Price),
        "low" => query.OrderBy(p => (double)p.Price),
        _ => query
      };

      query =
     string.IsNullOrEmpty(productParams.Search) ? query
     : query.Where(p => EF.Functions.Like(p.Name, $"%{productParams.Search}%"));

      return await PagedList<ProductDto>.CreateAsync(query, productParams.PageNumber, productParams.PageSize);

    }

    public void UpdateProduct(Product product)
    {
      _context.Entry(product).State = EntityState.Modified;
    }
  }
}