using System.Threading.Tasks;

namespace e_commerce.Interfaces
{
  public interface IUnitOfWork
  {
    IUserRepository UserRepository { get; }
    IProductRepo ProductRepo { get; }
    ICartRepo CartRepo { get; }
    Task<bool> Complete();
    bool HasChanges();
  }
}