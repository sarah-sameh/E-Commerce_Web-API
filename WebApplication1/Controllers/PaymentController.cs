using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IPaymentRepository paymentRepository;
        private readonly ICartRepository cartRepository;

        public PaymentController(UserManager<ApplicationUser> userManager, IPaymentRepository paymentRepository, ICartRepository cartRepository)
        {
            this.userManager = userManager;
            this.paymentRepository = paymentRepository;
            this.cartRepository = cartRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<GeneralResponse> GetAllPayment()
        {
            var paymentsWithUserNames = paymentRepository.GetAll()
              .Select(payment => new
              {
                  payment.Id,
                  payment.Date,
                  payment.Method,
                  payment.Amount,
                  CustomerName = payment.customer.UserName,
                  CustomerEmail = payment.customer.Email
              })
              .ToList();

            //return Ok(paymentsWithUserNames);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = paymentsWithUserNames
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
            var paymentsWithUserNames = paymentRepository.GetAll()
            .Where(payment => payment.customer.UserName == username)
            .Select(payment => new
            {
                payment.Id,
                payment.Date,
                payment.Method,
                payment.Amount,

                CustomerName = payment.customer.UserName,
                CustomerEmail = payment.customer.Email
            })
            .ToList();

            // return Ok(paymentsWithUserNames);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = paymentsWithUserNames
            };
            return response;
        }



        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GeneralResponse>> AddPayment(PaymentDTO paymentDTO)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not authenticated.");
            }
            if (ModelState.IsValid)
            {
                // var currentUser = await userManager.GetUserAsync(User);
                var Id = User.FindFirstValue(ClaimTypes.NameIdentifier);

                var payment = new Payment
                {
                    Date = paymentDTO.Date,
                    Method = paymentDTO.Method,
                    Amount = cartRepository.GetTotalPrice(Id),
                    customer = currentUser,
                    Customer_Id = currentUser.Id
                };

                paymentRepository.Insert(payment);
                paymentRepository.Save();
                // return Ok();
                return new GeneralResponse
                {
                    IsPass = true,
                    Message = new
                    {
                        payment.Id,
                        payment.Method,
                        payment.Amount

                    }
                };
            }
            else
            {
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Cant Add Payment"
                };
                return localresponse;
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<GeneralResponse> Edit(int id, PaymentDTO updatedPayment)
        {
            Payment OldPayment = paymentRepository.GetById(id);
            if (OldPayment == null)
            {
                GeneralResponse LocalResponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Not Found"
                };
                return LocalResponse;
            }
            OldPayment.Date = updatedPayment.Date;
            OldPayment.Method = updatedPayment.Method;
            OldPayment.Amount = updatedPayment.Amount;


            paymentRepository.Update(OldPayment);
            paymentRepository.Save();
            // return NoContent();
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = new
                {
                    OldPayment.Id,
                    OldPayment.Date,
                    OldPayment.Method,
                    OldPayment.Amount

                }
            };
            return response;

        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public ActionResult<GeneralResponse> Remove(int id)
        {
            try
            {
                paymentRepository.Delete(id);
                paymentRepository.Save();
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
