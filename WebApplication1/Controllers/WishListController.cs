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
    public class WishListController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IWishListRepository wishListRepository;

        public WishListController(UserManager<ApplicationUser> userManager, IWishListRepository wishListRepository)
        {
            this.userManager = userManager;
            this.wishListRepository = wishListRepository;
        }


        [HttpGet]
        [Authorize]
        public ActionResult<GeneralResponse> GetAll()
        {

            var wishListWithUserNames = wishListRepository.GetAll()

            .Select(wishList => new
            {
                wishList.Id,
                wishList.Product_Id,
                wishList.product.Name,
                wishList.product.Price,
                wishList.product.Description,

                CustomerName = wishList.customer.UserName,
                CustomerEmail = wishList.customer.Email
            })
            .ToList();

            //return Ok(wishListWithUserNames);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = wishListWithUserNames
            };
            return response;
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public ActionResult<GeneralResponse> GetById(int id)
        {
            var wishList = wishListRepository.GetById(id);

            if (wishList == null)
            {
                return NotFound();
            }


            var WishListWithProducts = new
            {
                wishList.Id,
                wishList.Product_Id,
                wishList.product.Name,
                wishList.product.Price,
                wishList.product.Description,
                CustomerName = wishList.customer.UserName,
                CustomerEmail = wishList.customer.Email
            };


            //  return Ok(WishListWithProducts);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = WishListWithProducts
            };
            return response;
        }


        [HttpGet("{username}")]
        [Authorize]
        public ActionResult<GeneralResponse> GetAllByUserName(string username)
        {
            var user = userManager.FindByNameAsync(username).Result;
            if (user == null)
            {
                return NotFound($"User '{username}' not found.");
            }
            var wishLists = wishListRepository.GetAll()
                .Where(wishList => wishList.customer.UserName == username)
                .Select(wishList => new WishListDTOs
                {
                    CustomerId = wishList.Customer_Id,
                    ProductName = wishList.product.Name,
                    ProductId = wishList.product.Id,
                    Price = wishList.product.Price
                })
                .ToList();

            // return Ok(wishListWithUserNames);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = wishLists
            };
            return response;
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GeneralResponse>> AddToWishList(WishListDTO wishListDTO)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not authenticated.");
            }
            if (ModelState.IsValid)
            {
                //var currentUser = await userManager.GetUserAsync(User);
                var wishList = new WishList
                {
                    Id = wishListDTO.Id,
                    Customer_Id = currentUser.Id,

                    Product_Id = wishListDTO.Product_Id,


                };
                wishListRepository.Insert(wishList);
                wishListRepository.Save();
                //  return Ok();
                return new GeneralResponse
                {
                    IsPass = true,
                    Message = "Done"

                };
            }
            else
            {
                // return BadRequest();
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Cant Add WishList"
                };
                return localresponse;
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<GeneralResponse> Edit(int id, WishListDTO updatedWishList)
        {
            WishList OldWishList = wishListRepository.GetById(id);
            if (OldWishList == null)
            {
                //  return NotFound();
                GeneralResponse LocalResponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Not Found"
                };
                return LocalResponse;
            }
            OldWishList.Product_Id = updatedWishList.Product_Id;





            wishListRepository.Update(OldWishList);
            wishListRepository.Save();
            //return NoContent();
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
                wishListRepository.Delete(id);
                wishListRepository.Save();
                //return NoContent();
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = true,
                    Message = "Done"
                };
                return Ok(localresponse);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);
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
