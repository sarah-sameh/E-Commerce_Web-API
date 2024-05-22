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
    }
}
