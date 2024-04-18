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
    public class ShipmentController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IShipmentRepository shipmentRepository;

        public ShipmentController(UserManager<ApplicationUser> userManager, IShipmentRepository shipmentRepository)
        {
            this.userManager = userManager;
            this.shipmentRepository = shipmentRepository;
        }
        [HttpGet]
        [Authorize]
        public ActionResult<GeneralResponse> GetAll()
        {
            var shipmentsWithUserNames = shipmentRepository.GetAll()
                .Select(shipment => new
                {
                    shipment.Id,
                    shipment.Date,
                    shipment.Address,
                    shipment.State,
                    shipment.City,
                    shipment.Zip_Code,
                    shipment.Country,
                    CustomerName = shipment.customer.UserName,
                    CustomerEmail = shipment.customer.Email
                })
                .ToList();

            // return Ok(shipmentsWithUserNames);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = shipmentsWithUserNames
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
            var shipmentsWithUserNames = shipmentRepository.GetAll()
            .Where(shipment => shipment.customer.UserName == username)
            .Select(shipment => new
            {
                shipment.Id,
                shipment.Date,
                shipment.Address,
                shipment.State,
                shipment.City,
                shipment.Zip_Code,
                shipment.Country,
                CustomerName = shipment.customer.UserName,
                CustomerEmail = shipment.customer.Email
            })
            .ToList();

            //return Ok(shipmentsWithUserNames);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = shipmentsWithUserNames
            };
            return response;
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<GeneralResponse>> AddShipment(ShipmentDTO shipmentDTO)
        {
            var currentUser = await userManager.GetUserAsync(User);
            if (currentUser == null)
            {
                return Unauthorized("User not authenticated.");
            }
            if (ModelState.IsValid)
            {
                //var currentUser = await userManager.GetUserAsync(User);

                var shipment = new Shipment
                {
                    Date = shipmentDTO.Date,
                    Address = shipmentDTO.Address,
                    State = shipmentDTO.State,
                    City = shipmentDTO.City,
                    Zip_Code = shipmentDTO.Zip_Code,
                    Country = shipmentDTO.Country,
                    customer = currentUser,
                    Customer_Id = currentUser.Id


                };

                shipmentRepository.Insert(shipment);
                shipmentRepository.Save();
                //   return Ok();
                return new GeneralResponse
                {
                    IsPass = true,
                    Message = new
                    {
                        shipment.Id,
                        shipment.Date,
                        shipment.Address,
                        shipment.City,
                        shipment.Country,
                        shipment.State,
                        shipment.Zip_Code

                    }
                };
            }
            else
            {
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Cant Add Shipment"
                };
                return localresponse;
            }
        }

        [HttpPut]
        [Authorize]
        public ActionResult<GeneralResponse> Edit(int id, ShipmentDTO updatedShipment)
        {
            Shipment OldShipment = shipmentRepository.GetById(id);
            if (OldShipment == null)
            {
                // return NotFound();
                GeneralResponse LocalResponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Not Found"
                };
                return LocalResponse;
            }
            OldShipment.Address = updatedShipment.Address;
            OldShipment.Date = updatedShipment.Date;
            OldShipment.Country = updatedShipment.Country;
            OldShipment.City = updatedShipment.City;
            OldShipment.State = updatedShipment.State;
            OldShipment.Zip_Code = updatedShipment.Zip_Code;



            shipmentRepository.Update(OldShipment);
            shipmentRepository.Save();
            // return NoContent();
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = new
                {
                    OldShipment.Id,
                    OldShipment.Date,
                    OldShipment.Address,
                    OldShipment.City,
                    OldShipment.Country,
                    OldShipment.State,
                    OldShipment.Zip_Code

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
                shipmentRepository.Delete(id);
                shipmentRepository.Save();
                // return NoContent();
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
