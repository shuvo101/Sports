using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SportsWeb.ViewModels
{
    public class TestViewModel
    {
        public int CoachID { get; set; }

        [Display(Name = "Test Type")]
        [Required(ErrorMessage = "Test Type is required")]
        public long TestTypeID { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Date is required")]
        [DisplayFormat(DataFormatString = "{0:ddMMyyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "Wrong format !!!!")]
        public DateTime TestDate { get; set; }

        public List<TestListViewModel> TestList = new List<TestListViewModel>();
    }

    public class TestListViewModel
    {
        public long ID { get; set; }
        public string TestName { get; set; }
        public string NumberOfPerticipent { get; set; }
        public DateTime TestDate { get; set; }
    }

    public class TestAthleteViewModel
    {
        public long ID { get; set; }
        public string TestName { get; set; }
        public DateTime TestDate { get; set; }
        public long AthleteID { get; set; }
        public int TestValue { get; set; }
        public List<TestAthleteListViewModel> TestAthleteList = new List<TestAthleteListViewModel>();
    }

    public class TestAthleteListViewModel
    {
        public long ID { get; set; }
        public long AthleteID { get; set; }
        public string AthleteName { get; set; }
        public int Distance { get; set; }
        public string FitnessRating { get; set; }
    }

    public class TestAthleteData
    {
        public string TestAthleteID { get; set; }
        public string Distance { get; set; }
    }

    public class DropDownViewModel
    {
        public long ID { get; set; }
        public string Name { get; set; }
    }

    public class AthleteTestViewModel
    {
        public string AthleteName { get; set; }
        public int TotalTest { get; set; }
        public List<AthleteTestListViewModel> TestList = new List<AthleteTestListViewModel>();
    }
    public class AthleteTestListViewModel
    {
        public long ID { get; set; }
        public string TestName { get; set; }
        public int TestValue { get; set; }
        public string FitnessRating{get; set;}
        public DateTime TestDate { get; set; }
    }
}
