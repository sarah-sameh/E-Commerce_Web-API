using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();

        Category GetById(int id);

        void Insert(Category obj);

        void Update(Category obj);

        void Delete(int id);

        void Save();
    }
}
