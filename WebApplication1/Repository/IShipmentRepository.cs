using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public interface IShipmentRepository
    {
        List<Shipment> GetAll();

        Shipment GetById(int id);

        void Insert(Shipment obj);

        void Update(Shipment obj);

        void Delete(int id);

        void Save();

    }
}