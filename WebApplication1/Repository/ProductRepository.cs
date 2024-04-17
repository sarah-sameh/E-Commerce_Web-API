using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class ProductRepository : IProductRepository
    {
        Context context;

        // inject Context
        public ProductRepository(Context _context)//ask context not create 
        {
            context = _context;
        }
        public List<Product> GetAll()
        {
            return context.products
                .Include(p => p.category)
                .ToList();
        }

        public Product GetById(int id)
        {
            return context.products
                .Include(p => p.category)
                .FirstOrDefault(p => p.Id == id);
        }
        public void Insert(Product obj)
        {
            context.Add(obj);
        }
        public void Update(Product obj)
        {
            context.Update(obj);
        }

        public void Delete(int id)
        {
            Product crs = GetById(id);
            context.Remove(crs);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
