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

        //[HttpGet]
        //public ActionResult Get(int id)
        //{

        //}

        //[HttpPost]
        //[Authorize]

        //public ActionResult Create()
        //{

        //}

        //[HttpPut]
        //public ActionResult Update(int id)
        //{

        //}

        //[HttpDelete]
        //public ActionResult Delete(int id)
        //{

        //}

    }
}
