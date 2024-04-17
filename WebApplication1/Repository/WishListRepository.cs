using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class WishListRepository : IWishListRepository
    {
        Context context;

        // inject Context
        public WishListRepository(Context _context)//ask context not create 
        {
            context = _context;
        }
        public List<WishList> GetAll()
        {
            return context.wishLists
                .Include(p => p.customer)
                .Include(p => p.product)
                .ToList();
        }

        public WishList GetById(int id)
        {
            return context.wishLists
                .Include(p => p.product)
                .Include(p => p.customer)
                .FirstOrDefault(p => p.Id == id);
        }
        public void Insert(WishList obj)
        {
            context.Add(obj);
        }
        public void Update(WishList obj)
        {
            context.Update(obj);
        }

        public void Delete(int id)
        {
            WishList crs = GetById(id);
            crs.IsDeleted = true;
            Update(crs);

        }

        public void Save()
        {
            context.SaveChanges();
        }
        public List<WishList> GetAllbyCustomerId(string id)
        {
            List<WishList> wishLists = context.wishLists.
                Where(item => item.Customer_Id == id && item.IsDeleted == false).ToList();
            return wishLists;
        }

    }
}
