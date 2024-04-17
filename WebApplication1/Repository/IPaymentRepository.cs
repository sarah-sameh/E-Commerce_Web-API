using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IPaymentRepository
    {
        List<Payment> GetAll();

        Payment GetById(int id);

        void Insert(Payment obj);

        void Update(Payment obj);

        void Delete(int id);

        void Save();
    }

}
