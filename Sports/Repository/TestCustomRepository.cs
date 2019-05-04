using Microsoft.EntityFrameworkCore;
using Sports.API.Data;
using Sports.API.Model;
using Sports.API.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.API.Repository
{
    public class TestCustomRepository : GenericRepository<Test>
    {
        private readonly ApplicationDbContext _context;

        public TestCustomRepository(ApplicationDbContext context) : base(context)
        {
            _context = _context ?? context;
        }

        internal List<TestListViewModel> GetAllTestByUserID(long id)
        {
            var list = _context.Test.Where(t => !t.IsRemoved && t.CoachID == id).
                Select(t => new TestListViewModel
                {
                    ID = t.ID,
                    NumberOfPerticipent = t.TestAthletes.Count(),
                    TestDate = t.TestDate,
                    TestName = t.TestType.Name
                });

            return list.OrderByDescending(t=>t.TestDate).ToList();
        }

        internal List<DropDownViewModel> GetAllAthletesByCoachID(long id)
        {
            var list = _context.CoachAthlete.Where(t => !t.IsRemoved && t.CoachID == id).
                Select(t => new DropDownViewModel
                {
                    ID = t.AthleteID,
                    Name = string.Format("{0} {1}",t.Athlete.FirstName, t.Athlete.LastName)
                });

            return list.ToList();
        }
        
        internal TestAthleteViewModel GetAthleteListByTestID(long id)
        {
            var item = _context.Test.Where(t => !t.IsRemoved && t.ID == id).
                Select(t => new TestAthleteViewModel
                {
                    ID = t.ID,
                    TestDate = t.TestDate,
                    TestName = t.TestType.Name,
                    TestAthleteList = _context.TestAthlete.Where(a => !a.IsRemoved && a.TestID == t.ID).Select(
                        ta => new TestAthleteListViewModel {
                            ID = ta.ID,
                            AthleteID = ta.AthleteID,
                            AthleteName = string.Format("{0} {1}", ta.Athlete.FirstName, ta.Athlete.LastName),
                            Distance = ta.TestValue,
                            FitnessRating = ta.FitnessRating.Name

                        }).OrderByDescending(ta=>ta.Distance).ToList()

                }).FirstOrDefault();
            return item;
        }

        internal long GetFitnessRatingID(int testValue)
        {
            var item =_context.FitnessRating.FirstOrDefault(f => testValue > f.MinValue && testValue <= f.MaxValue);
            return item == null ? 1 : item.ID; 

        }

        internal bool DeleteTest(long iD,string userId)
        {
            bool retVal = true;
            var test = _context.Test.FirstOrDefault(t => t.ID == iD);
            var testAthletes = _context.TestAthlete.Where(a => a.TestID == iD).ToList();
            foreach (var item in testAthletes)
            {
                item.IsRemoved = true;
                item.UpdatedBy = Convert.ToInt32(userId);
                item.UpdatedDate = DateTime.Now;
            }
            test.IsRemoved = true;
            test.UpdatedBy = Convert.ToInt32(userId);
            test.UpdatedDate = DateTime.Now;
            _context.SaveChanges();
            return retVal;
        }

        internal Test GetTestByCoachAndDate(int coachID, DateTime testDate, long testTypeId)
        {
            return _context.Test.FirstOrDefault(t => t.CoachID == coachID && t.TestDate.Date == testDate.Date && t.TestTypeID == testTypeId && !t.IsRemoved);
        }

        internal AthleteTestViewModel GetAllTestByAthleteID(long id)
        {
            AthleteTestViewModel item = new AthleteTestViewModel();
            List<AthleteTestListViewModel> testList = _context.TestAthlete.Where(t => t.AthleteID == id && !t.IsRemoved).Select(a =>
               new AthleteTestListViewModel
               {
                   ID = a.ID,
                   TestDate = a.Test.TestDate,
                   FitnessRating = a.FitnessRating.Name,
                   TestName = a.Test.TestType.Name,
                   TestValue = a.TestValue
               }).ToList();
            item.TestList = testList.OrderByDescending(t=>t.TestDate).ToList();
            item.TotalTest = testList.Count;
            return item;
        }

        internal TestAthlete GetTestAthleteByTestAndAthlete(long iD, int athleteID)
        {
            return _context.TestAthlete.Include(p=>p.Athlete).FirstOrDefault(t => t.TestID == iD && t.AthleteID == athleteID && !t.IsRemoved);
        }
    }
}
