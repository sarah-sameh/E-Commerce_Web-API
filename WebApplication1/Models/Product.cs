using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Description { get; set; }
        public List<Cart>? carts { get; set; }
        public List<WishList>? wishLists { get; set; }

        [ForeignKey("category")]
        public int? Category_Id { get; set; }

        public Category? category { set; get; }
    }
}
