using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly ICartRepository cartRepository;

        public CartController(UserManager<ApplicationUser> userManager, ICartRepository cartRepository)
        {
            this.userManager = userManager;
            this.cartRepository = cartRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<GeneralResponse> GetAll()
        {
            var cartsWithProductNames = cartRepository.GetAll()
                 .Select(cart => new
                 {
                     cart.Id,
                     cart.Quantity,
                     cart.Product_Id,
                     CustomerName = cart.customer.UserName,
                     CustomerEmail = cart.customer.Email,
                     ProductNames = cart.product.Name
                 })
                 .ToList();


            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = cartsWithProductNames
            };
            return response;

        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GeneralResponse>> AddToCart(CartDTO cartDTO)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not authenticated.");
            }
            if (ModelState.IsValid)
            {
                //var currentUser = await userManager.GetUserAsync(User);
                var cart = new Cart
                {
                    Quantity = cartDTO.Quantity,
                    Product_Id = cartDTO.Product_Id,
                    Customer_Id = currentUser.Id,

                };
                cartRepository.Insert(cart);
                cartRepository.Save();
                GeneralResponse response = new GeneralResponse()
                {
                    IsPass = true,
                    Message = "Done"
                };
                return response;
            }
            else
            {
                GeneralResponse response = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Cant Add To Cart"
                };
                return response;
            }
        }
        [HttpPut]
        [Authorize]
        public ActionResult<GeneralResponse> Edit(int id, CartDTO updatedCart)
        {
            Cart OldCart = cartRepository.GetById(id);
            if (OldCart == null)
            {
                return NotFound();
            }
            OldCart.Quantity = updatedCart.Quantity;
            OldCart.Product_Id = updatedCart.Product_Id;




            cartRepository.Update(OldCart);
            cartRepository.Save();
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = "Done"
            };
            return response;
        }



        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult<GeneralResponse> RemoveFromCart(int id)
        {
            try
            {
                cartRepository.Delete(id);
                cartRepository.Save();
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = true,
                    Message = "Done"
                };
                return Ok(localresponse);
            }
            catch (Exception ex)
            {
                GeneralResponse response = new GeneralResponse()
                {
                    IsPass = false,
                    Message = ex.Message

                };
                return StatusCode(500, response);
            }
        }

    }
}
