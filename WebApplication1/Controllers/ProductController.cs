using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository productRepository;


        public ProductController(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<GeneralResponse> GetAll()
        {
            List<Product> products = productRepository.GetAll();
            List<ProductDTO> productDTOs = products.Select(product => new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Category_Id = product.Category_Id,

            }).ToList();
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = productDTOs
            };
            return response;


        }

        [HttpGet("{id:int}")]
        [Authorize]
        public ActionResult<GeneralResponse> GetById(int id)
        {
            Product product = productRepository.GetById(id);

            if (product == null)
            {
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Id Not Found"
                };
                return localresponse;
            }
            var productDTo = new ProductDTO
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Description = product.Description,
                Category_Id = product.Category_Id

            };

            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = productDTo
            };
            return response;


        }



        [HttpPost]
        [Authorize]
        public ActionResult<GeneralResponse> AddProduct(ProductDTO newProductDTO)
        {
            if (ModelState.IsValid == true)
            {
                var newProduct = new Product
                {
                    Name = newProductDTO.Name,
                    Price = newProductDTO.Price,
                    Description = newProductDTO.Description,
                    Category_Id = newProductDTO.Category_Id,
                };
                productRepository.Insert(newProduct);
                productRepository.Save();


                return new GeneralResponse
                {
                    IsPass = true,
                    Message = new
                    {
                        newProduct.Id,
                        newProduct.Name,
                        newProduct.Price,
                        newProduct.Description
                    }
                };

            }
            GeneralResponse localresponse = new GeneralResponse()
            {
                IsPass = false,
                Message = "Cant Add"
            };
            return localresponse;
        }




        [HttpPut]
        [Authorize]
        public ActionResult<GeneralResponse> Edit(int id, ProductDTO updatedProduct)
        {
            Product Oldproduct = productRepository.GetById(id);

            if (Oldproduct == null)
            {

                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Id Not Found"
                };
                return localresponse;
            }

            Oldproduct.Name = updatedProduct.Name;
            Oldproduct.Price = updatedProduct.Price;
            Oldproduct.Description = updatedProduct.Description;
            Oldproduct.Category_Id = updatedProduct.Category_Id;
            productRepository.Update(Oldproduct);
            productRepository.Save();
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = new
                {
                    Oldproduct.Id,
                    Oldproduct.Name,
                    Oldproduct.Price,
                    Oldproduct.Description
                }
            };
            return response;

        }



        [HttpDelete("{id:int}")]
        [Authorize]
        public IActionResult Remove(int id)
        {
            try
            {
                productRepository.Delete(id);
                productRepository.Save();
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = true,
                    Message = "done"
                };
                return Ok(localresponse);
            }
            catch (Exception ex)
            {
                //return BadRequest(ex.Message);

                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = ex.Message

                };
                return StatusCode(500, localresponse);
            }
        }


    }
}
