using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.API.Model
{
    [Table("Test")]
    public class Test : BaseModel
    {   
        [Display(Name = "Test Type")]
        [Required(ErrorMessage = "Test Type is required")]
        public long TestTypeID { get; set; }
        public virtual TestType TestType { get; set; }

        [Display(Name = "Date")]
        [Required(ErrorMessage = "Date is required")]
        [DisplayFormat(DataFormatString = "{0:ddMMyyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date, ErrorMessage = "Wrong format !!!!")]
        public DateTime TestDate { get; set; }
        
        [Display(Name = "Coach")]
        [Required(ErrorMessage = "Coach is required")]
        public int CoachID { get; set; }
        public virtual ApplicationUser Coach { get; set; }

        public virtual List<TestAthlete> TestAthletes { get; set; }

    }
}
