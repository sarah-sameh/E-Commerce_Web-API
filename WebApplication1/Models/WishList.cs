using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class WishList
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        [ForeignKey("customer")]
        public string Customer_Id { get; set; }
        public ApplicationUser customer { get; set; }
        [ForeignKey("product")]
        public int? Product_Id { get; set; }
        public Product? product { get; set; }
    }
}
