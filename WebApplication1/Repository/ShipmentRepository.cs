using Microsoft.EntityFrameworkCore;
using WebApplication1.Models;

namespace WebApplication1.Repository
{
    public class ShipmentRepository : IShipmentRepository
    {
        Context context;

        // inject Context
        public ShipmentRepository(Context _context)
        {
            context = _context;
        }
        public List<Shipment> GetAll()
        {
            return context.shipments.Include(s => s.customer).ToList();
        }

        public Shipment GetById(int id)
        {
            return context.shipments

                .FirstOrDefault(p => p.Id == id);
        }
        public void Insert(Shipment obj)
        {
            context.Add(obj);
        }
        public void Update(Shipment obj)
        {
            context.Update(obj);
        }

        public void Delete(int id)
        {
            Shipment crs = GetById(id);
            context.Remove(crs);
        }

        public void Save()
        {
            context.SaveChanges();
        }
    }
}

