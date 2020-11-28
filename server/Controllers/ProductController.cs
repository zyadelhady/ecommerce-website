using System.Threading.Tasks;
using AutoMapper;
using e_commerce.DTOs;
using e_commerce.Entities;
using e_commerce.Extensions;
using e_commerce.Helpers;
using e_commerce.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace e_commerce.Controllers
{
  public class ProductController : BaseApiController
  {
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICollectionRepo _collectionRepo;
    private readonly IMapper _mapper;
    public ProductController(IUnitOfWork unitOfWork, ICollectionRepo collectionRepo, IMapper mapper)
    {
      _mapper = mapper;
      _collectionRepo = collectionRepo;
      _unitOfWork = unitOfWork;
    }


    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> CreateProduct(CreateProductDto createProductDto)
    {
      if (createProductDto.Price < 1 || createProductDto.Quantity < 1)
        return BadRequest("Price and quantity can not be less than 0");

      var product = _mapper.Map<Product>(createProductDto);

      product.Collection = await _collectionRepo.GetCollectionByNameAsync(createProductDto.CollectionName);

      _unitOfWork.ProductRepo.AddProduct(product);

      return (await _unitOfWork.Complete()) ? product : BadRequest("Error creating product.");
    }

    [HttpGet]
    public async Task<ActionResult<PagedList<ProductDto>>> GetProducts([FromQuery] ProductParams productsParams)
    {
      var products = await _unitOfWork.ProductRepo.GetProductsAsync(productsParams);

      Response.AddPaginationHeader(products.CurrentPage, products.PageSize, products.TotalCount, products.TotalPages);

      return products;
    }

    [HttpGet("{slug}")]
    public async Task<ActionResult<ProductDto>> GetProduct(string slug)
    {
      var product = await _unitOfWork.ProductRepo.GetProductBySlugAsync(slug);

      return product == null ? NotFound("No product with that slug.") : product;
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
      var product = await _unitOfWork.ProductRepo.GetProductByIdAsync(id);

      if (product == null) return BadRequest("No product with that id.");

      _unitOfWork.ProductRepo.DeleteProduct(product);

      return (await _unitOfWork.Complete()) ? NoContent() : BadRequest("Can not delete product.");
    }

    [HttpPatch("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<Product>> UpdateProduct(string id, UpdateProductDto updateProductDto)
    {
      var product = await _unitOfWork.ProductRepo.GetProductByIdAsync(id);

      if (product == null) return BadRequest("No product with that id.");

      _mapper.Map(updateProductDto, product);

      if (product.Price < 1 || product.Quantity < 1)
        return BadRequest("Price and quantity can not be less than 0");

      _unitOfWork.ProductRepo.UpdateProduct(product);

      return (await _unitOfWork.Complete()) ? product : BadRequest("Can not delete product.");
    }

  }
}