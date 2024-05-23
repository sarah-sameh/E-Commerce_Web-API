using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.DTO;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
 
    [Route("api/[controller]")]
    [ApiController]
    public class MyController : ControllerBase
    {
        private readonly IMyRepository myRepository;
        public MyController(IMyRepository _myRepository)
        {
            this.myRepository = _myRepository;
        }

        [HttpGet]
        [Authorize]
        public ActionResult GetAll()
        {
           List<MyModel> myMod = myRepository.GetAll();
           List<MyDTO> myDTOs = new List<MyDTO>();
           foreach(var item in myMod)
            {
                var model = new MyDTO();
                model.Id = item.Id;
                model.Name = item.Name;
                model.Department = item.Department;
                model.Salary = item.Salary;
                myDTOs.Add(model);
                
            }
           return Ok(myDTOs);
           
        }

          [HttpGet]
           public ActionResult GetById(int? id)
          {
            if(id == null)
            {
                return BadRequest();
            }
            MyModel myMod = myRepository.Get((int)id);
            var model = new MyDTO
            {
                Id = myMod.Id,
                Name = myMod.Name,
                Department = myMod.Department,
                Salary = myMod.Salary

            };
            return Ok(model);
           
          }

            [HttpPost]
            [Authorize]

            public ActionResult Create(MyDTO objDTO)
            {
               if(ModelState.IsValid == true)
            {
                MyModel mymod = new MyModel()
                {
                    Name = objDTO.Name,
                    Department = objDTO.Department,
                    Salary = objDTO.Salary
                };
                myRepository.Create(mymod);
                myRepository.Save();
                return Ok(mymod);

            }
               return BadRequest();
            }

            [HttpPut]
            public ActionResult Update(MyDTO modelDTO)
            {
               MyModel oldModel = myRepository.Get(modelDTO.Id);
               if(oldModel == null)
            {
                return BadRequest();
            }
               oldModel.Name = modelDTO.Name;
               oldModel.Department = modelDTO.Department;
               oldModel.Salary = modelDTO.Salary;
               myRepository.Update(oldModel);
               myRepository.Save();
               return Ok(oldModel);
            }

            [HttpDelete]
            public ActionResult Delete(int? id)
            {
            
                if(id == null)
                {
                    return BadRequest();
                }
            myRepository.Delete((int)id);
            return Ok();

            
            }

    }
}
