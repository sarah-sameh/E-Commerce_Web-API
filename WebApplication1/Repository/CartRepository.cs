using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class CartRepository : ICartRepository
    {
        Context context;

        // inject Context
        public CartRepository(Context _context)//ask context not create 
        {
            context = _context;
        }
        public List<Cart> GetAll()
        {
            return context.carts
                 .Include(c => c.product)
                 .Include(c => c.customer)
                 .Where(c => !c.IsDeleted)
                 .ToList();
        }

        public Cart GetById(int id)
        {
            return context.carts
                 .Include(c => c.product)
                .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
        }

        public List<Cart> GetCartItemsOfCustomer(string customerId)
        {

            return GetAll().Where(items => items.Customer_Id == customerId).ToList();
        }
        public int GetTotalPrice(string customerId)
        {
            int totalPrice = 0;
            foreach (Cart cartItem in GetCartItemsOfCustomer(customerId))
            {
                totalPrice += (cartItem.product.Price * cartItem.Quantity);
            }

            return totalPrice;
        }

        public void Insert(Cart obj)
        {
            context.Add(obj);
        }
        public void Update(Cart obj)
        {
            context.Update(obj);
        }

        public void Delete(int id)
        {
            Cart crs = GetById(id);
            crs.IsDeleted = true;
            Update(crs);


        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
