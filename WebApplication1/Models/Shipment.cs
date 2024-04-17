using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Shipment
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip_Code { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("customer")]
        public string Customer_Id { get; set; }
        public ApplicationUser customer { get; set; }

    }
}
