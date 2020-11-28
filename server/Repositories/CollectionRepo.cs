using System.Threading.Tasks;
using e_commerce.Data;
using e_commerce.Entities;
using e_commerce.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Repositories
{
  public class CollectionRepo : ICollectionRepo
  {
    private readonly DataContext _context;

    public CollectionRepo(DataContext context)
    {
      _context = context;
    }

    public async Task<Collection> GetCollectionByNameAsync(string name)
    {
      return await _context.Collections.FirstOrDefaultAsync(c => c.Name == name);
    }
  }
}