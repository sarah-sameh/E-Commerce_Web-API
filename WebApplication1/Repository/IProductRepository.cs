using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IProductRepository
    {
        List<Product> GetAll();

        Product GetById(int id);

        void Insert(Product obj);

        void Update(Product obj);

        void Delete(int id);

        void Save();
    }
}