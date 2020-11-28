using System.Threading.Tasks;
using e_commerce.Entities;

namespace e_commerce.Interfaces
{
  public interface ICollectionRepo
  {
    Task<Collection> GetCollectionByNameAsync(string name);
  }
}