using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        Context context;

        // inject Context
        public CategoryRepository(Context _context)//ask context not create 
        {
            context = _context;
        }
        public List<Category> GetAll()
        {
            return context.categories.Include(c => c.products).ToList();
        }

        public Category GetById(int id)
        {
            return context.categories
                 .Include(c => c.products)
                .FirstOrDefault(p => p.Id == id);
        }
        public void Insert(Category obj)
        {
            context.Add(obj);
        }
        public void Update(Category obj)
        {
            context.Update(obj);
        }

        public void Delete(int id)
        {
            Category crs = GetById(id);
            context.Remove(crs);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
