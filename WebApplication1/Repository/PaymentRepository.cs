using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class PaymentRepository : IPaymentRepository
    {
        Context context;

        // inject Context
        public PaymentRepository(Context _context)//ask context not create 
        {
            context = _context;
        }
        public List<Payment> GetAll()
        {
            return context.payments.Include(c => c.customer).ToList();
        }

        public Payment GetById(int id)
        {
            return context.payments
                 .Include(c => c.customer)
                .FirstOrDefault(p => p.Id == id);
        }
        public void Insert(Payment obj)
        {
            context.Add(obj);
        }
        public void Update(Payment obj)
        {
            context.Update(obj);
        }

        public void Delete(int id)
        {
            Payment crs = GetById(id);
            context.Remove(crs);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}
