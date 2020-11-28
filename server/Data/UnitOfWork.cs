using System.Threading.Tasks;
using AutoMapper;
using e_commerce.Interfaces;
using e_commerce.Repositories;

namespace e_commerce.Data
{
  public class UnitOfWork : IUnitOfWork
  {
    private readonly DataContext _context;
    private readonly IMapper _mapper;
    public UnitOfWork(DataContext context, IMapper mapper)
    {
      _mapper = mapper;
      _context = context;
    }

    public IUserRepository UserRepository => new UserRepository(_context, _mapper);
    public IProductRepo ProductRepo => new ProductRepo(_context, _mapper);
    public ICartRepo CartRepo => new CartRepo(_context, _mapper);

    public async Task<bool> Complete()
    {
      return await _context.SaveChangesAsync() > 0;
    }

    public bool HasChanges()
    {
      return _context.ChangeTracker.HasChanges();
    }
  }
}