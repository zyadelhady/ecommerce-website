namespace e_commerce.Helpers
{

  public class ProductParams : PaginationParams
  {
    public string CollectionName { get; set; }
    public string Price { get; set; }
    public string Search { get; set; }
  }

}