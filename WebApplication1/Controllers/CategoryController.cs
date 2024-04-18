using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult<GeneralResponse> GetAllCategory()
        {
            List<Category> categories = categoryRepository.GetAll();
            List<CategoryWithProduct> categoriesWithProduct = categories.Select(category =>
                new CategoryWithProduct
                {
                    Id = category.Id,
                    Category_Name = category.Name,
                    ProductNames = category.products?.Select(p => p.Name).ToList()
                }).ToList();
            //return Ok(categoriesWithProduct);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = categoriesWithProduct
            };
            return response;
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public ActionResult<GeneralResponse> GetById(int id)
        {
            var category = categoryRepository.GetById(id);

            if (category == null)
            {
                // return NotFound();
                GeneralResponse LocalResponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Not Found"
                };
                return LocalResponse;
            }

            var categoryWithProducts = new CategoryWithProduct
            {
                Id = category.Id,
                Category_Name = category.Name,
                ProductNames = category.products.Select(p => p.Name).ToList()
            };


            // return Ok(categoryWithProducts);
            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = categoryWithProducts
            };
            return response;
        }

        [HttpPost]
        [Authorize]
        public ActionResult<GeneralResponse> AppCategory(CatDTO catDTO)
        {
            if (ModelState.IsValid == true)
            {
                var category = new Category
                {
                    Name = catDTO.Name
                    //  products = catDTO.ProductNames.Select(name => new Product { Name = name }).ToList()
                };
                categoryRepository.Insert(category);
                categoryRepository.Save();
                // return CreatedAtAction(nameof(GetById), new { id = category.Id }, category);
                return new GeneralResponse
                {
                    IsPass = true,
                    Message = new
                    {
                        category.Id,
                        category.Name,

                    }
                };
            }
            else
            {
                GeneralResponse localresponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Cant Add Category"
                };
                return localresponse;
            }


        }

        [HttpPut]
        [Authorize]
        public ActionResult<GeneralResponse> Edit(int id, CatDTO UpdatedCategory)
        {
            Category OldCategory = categoryRepository.GetById(id);
            if (OldCategory == null)
            {
                // return NotFound();
                GeneralResponse LocalResponse = new GeneralResponse()
                {
                    IsPass = false,
                    Message = "Not Found"
                };
                return LocalResponse;
            }
            OldCategory.Name = UpdatedCategory.Name;
            categoryRepository.Update(OldCategory);
            categoryRepository.Save();

            GeneralResponse response = new GeneralResponse()
            {
                IsPass = true,
                Message = new
                {
                    OldCategory.Id,
                    OldCategory.Name,

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
                categoryRepository.Delete(id);
                categoryRepository.Save();
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
