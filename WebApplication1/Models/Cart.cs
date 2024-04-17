using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int Quantity { get; set; }
        public bool IsDeleted { get; set; } = false;

        // Change Customer_Id to string
        [ForeignKey("customer")]
        public string Customer_Id { get; set; }

        public ApplicationUser customer { get; set; }

        [ForeignKey("product")]
        public int Product_Id { get; set; }
        public Product? product { get; set; }
    }
}
