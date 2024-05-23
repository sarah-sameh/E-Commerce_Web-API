using System.Security.Cryptography;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IMyRepository
    {
        List<MyModel> GetAll();
        MyModel Get(int id);

        void Create(MyModel obj);

        void Update(MyModel obj);

        void Delete(int id);

        void Save();


    }
}
