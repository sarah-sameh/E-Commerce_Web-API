using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class MyRepository : IMyRepository
    {
        Context context;

        public MyRepository(Context _context)
        {
            context = _context;
        }
        public List<MyModel> GetAll()
        {
            return context.myModels.ToList();  
        }

        public MyModel Get(int id)
        {
            return context.myModels.FirstOrDefault(m => m.Id == id);
        }

        public void Create(MyModel obj)
        {
            context.Add(obj);
        }

        public void Update(MyModel obj)
        {
            context.Update(obj);
        }

        public void Delete(int id)
        {
           MyModel model = Get(id);
            context.Remove(model);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
