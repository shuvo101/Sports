using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Sports.API.Data;
using Sports.API.Model;
using Sports.API.Repository;
using Sports.API.ViewModels;

namespace Sports.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly UnitOfWork _repo;
        private ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TestController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
            _repo = new UnitOfWork(_context);
        }

        #region All Get APIs

        [HttpGet]
        [Authorize(Policy = "Normal")]
        [Route("[action]")]
        public ActionResult<AthleteTestViewModel> GetTestListByAthlete(long id)
        {
            AthleteTestViewModel model = new AthleteTestViewModel();
            model = _repo.TestCustomRepository.GetAllTestByAthleteID(id);
            return Ok(model);
        }

        // GET: api/Test
        [HttpGet]
        [Authorize(Policy = "TestOperation")]
        [Route("[action]")]
        public ActionResult<List<TestListViewModel>> GetTestListByCoach(long id)
        {
            List<TestListViewModel> model = new List<TestListViewModel>();
            model = _repo.TestCustomRepository.GetAllTestByUserID(id);
            return Ok(model);
        }

        [HttpGet]
        [Authorize(Policy = "TestOperation")]
        [Route("[action]")]
        public ActionResult<TestAthleteViewModel> GetAthleteListByTestID(long id)
        {
            TestAthleteViewModel model = new TestAthleteViewModel();
            model = _repo.TestCustomRepository.GetAthleteListByTestID(id);
            return Ok(model);
        }

        [HttpGet]
        [Authorize(Policy = "TestOperation")]
        [Route("[action]")]
        public ActionResult<List<DropDownViewModel>> GetTestTypes()
        {
            List<DropDownViewModel> model = new List<DropDownViewModel>();
            model = _repo.TestTypeRepository.GetAll().Select(
                t=> new DropDownViewModel {
                    ID = t.ID,
                    Name = t.Name
                }).ToList();
            return Ok(model);
        }

        [HttpGet]
        [Authorize(Policy = "TestOperation")]
        [Route("[action]")]
        public ActionResult<List<DropDownViewModel>> GetAllAthletesByCoachID(long id)
        {
            List<DropDownViewModel> model = new List<DropDownViewModel>();
            model = _repo.TestCustomRepository.GetAllAthletesByCoachID(id);
            return Ok(model);
        }

        #endregion

        #region All Post APIs
        [HttpPost]
        [Authorize(Policy = "TestOperation")]
        [Route("[action]")]
        public ActionResult<PayloadResponse> CreateTest([FromBody] TestViewModel info)
        {
            PayloadResponse response = new PayloadResponse();
            

            Test test = new Test();
            if(info == null)
            {
                response.Message = "Data did not send properly";
                response.Payload = test;
                response.PayloadType = "CreateTest";
                response.ResponseTime = DateTime.Now.ToString();
                response.Success = false;
                return response;
            }

            Test existingTest = _repo.TestCustomRepository.GetTestByCoachAndDate(info.CoachID, info.TestDate,info.TestTypeID);
            if (existingTest != null && existingTest.ID > 0)
            {
                response.Message = string.Format("You already have a Test at same Date ({0}) ",info.TestDate);
                response.Payload = test;
                response.PayloadType = "CreateTest";
                response.ResponseTime = DateTime.Now.ToString();
                response.Success = false;
                return response;
            }

            test.CoachID = info.CoachID;
            test.TestDate = info.TestDate;
            test.TestTypeID = info.TestTypeID;
            test.CreatedBy = Convert.ToInt32(User.Identity.Name);
            test.CreatedDate = DateTime.Now;
            test.IsRemoved = false;
            _repo.TestRepository.Add(test);
            _repo.Save();

            response.Message = "Test is successfully created.";
            response.Payload = null;
            response.PayloadType = "CreateTest";
            response.ResponseTime = DateTime.Now.ToString();
            response.Success = true;

            return response;
        }

        [HttpPost]
        [Authorize(Policy = "TestOperation")]
        [Route("[action]")]
        public ActionResult<PayloadResponse> CreateTestAthlete([FromBody] TestAthleteViewModel info)
        {
            PayloadResponse response = new PayloadResponse();
            TestAthlete testAthlete = new TestAthlete();
            if (info == null)
            {
                response.Message = "Data did not send properly";
                response.Payload = testAthlete;
                response.PayloadType = "CreateTestAthlete";
                response.ResponseTime = DateTime.Now.ToString();
                response.Success = false;
                return response;
            }

            TestAthlete existingTest = _repo.TestCustomRepository.GetTestAthleteByTestAndAthlete(info.ID, info.AthleteID);
            if (existingTest != null && existingTest.ID > 0)
            {
                response.Message = string.Format("The Athlete ({0} {1}) is already exist into the Test. ", existingTest.Athlete.FirstName, existingTest.Athlete.LastName);
                response.Payload = testAthlete;
                response.PayloadType = "CreateTestAthlete";
                response.ResponseTime = DateTime.Now.ToString();
                response.Success = false;
                return response;
            }

            testAthlete.AthleteID = info.AthleteID;
            testAthlete.TestValue = info.TestValue;
            testAthlete.TestID = info.ID;
            testAthlete.CreatedBy = Convert.ToInt32(User.Identity.Name);
            testAthlete.CreatedDate = DateTime.Now;
            testAthlete.IsRemoved = false;
            testAthlete.FitnessRatingID = _repo.TestCustomRepository.GetFitnessRatingID(testAthlete.TestValue);
            _repo.TestAthleteRepository.Add(testAthlete);
            _repo.Save();

            response.Message = "Athlete is successfully added in Test.";
            response.Payload = null;
            response.PayloadType = "CreateTestAthlete";
            response.ResponseTime = DateTime.Now.ToString();
            response.Success = true;
            return response;
        }

        [HttpPost]
        [Authorize(Policy = "TestOperation")]
        [Route("[action]")]
        public ActionResult<PayloadResponse> EditTestAthlete([FromBody] TestAthleteListViewModel info)
        {
            PayloadResponse response = new PayloadResponse();
            response.RequestTime = DateTime.Now.ToString();
            
            TestAthlete athlete = _repo.TestAthleteRepository.GetAll().Where(x=> x.ID == info.ID && !x.IsRemoved).FirstOrDefault();
            
            if (info == null && info.ID <= 0)
            {
                response.Message = "Data did not send properly";
                response.Payload = athlete;
                response.PayloadType = "EditTestAthlete";
                response.ResponseTime = DateTime.Now.ToString();
                response.Success = false;
                return response;
            }
            athlete.TestValue = info.Distance;
            athlete.UpdatedBy = Convert.ToInt32(User.Identity.Name);
            athlete.UpdatedDate = DateTime.Now;
            athlete.FitnessRatingID = _repo.TestCustomRepository.GetFitnessRatingID(info.Distance);
            _repo.TestAthleteRepository.Update(athlete);
            
            _repo.Save();

            response.Message = "Athlete is successfully updated.";
            response.Payload = null;
            response.PayloadType = "EditTestAthlete";
            response.ResponseTime = DateTime.Now.ToString();
            response.Success = true;
            return response;
        }

        [HttpPost]
        [Authorize(Policy = "TestOperation")]
        [Route("[action]")]
        public ActionResult DeleteAthlete([FromBody] TestAthleteListViewModel info)
        {
            TestAthlete athlete = _repo.TestAthleteRepository.GetAll().Where(x => x.ID == info.ID && !x.IsRemoved).FirstOrDefault();

            if (info == null && info.ID <= 0)
            {
                return BadRequest();
            }
            athlete.IsRemoved = true;
            _repo.TestAthleteRepository.Update(athlete);
            _repo.Save();
            info.AthleteID = athlete.ID;
            info.ID = athlete.ID;
            return Ok(info);
        }

        [HttpPost]
        [Authorize(Policy = "TestOperation")]
        [Route("[action]")]
        public ActionResult DeleteTest([FromBody] TestAthleteListViewModel info)
        {
            Test test = _repo.TestRepository.GetAll().Where(x => x.ID == info.ID && !x.IsRemoved).FirstOrDefault();

            if (info == null && info.ID <= 0)
            {
                return BadRequest();
            }
            //test.IsRemoved = true;
            //_repo.TestRepository.Update(test);
            //_repo.Save();

            bool isDelete = _repo.TestCustomRepository.DeleteTest(info.ID,User.Identity.Name);

            info.AthleteID = test.ID;
            info.ID = test.ID;
            return Ok(info);
        }

        #endregion
        
    }
}
