using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.API.Model
{
    [Table("TestAthlete")]
    public class TestAthlete : BaseModel
    {
        [Display(Name = "Test")]
        [Required(ErrorMessage = "Test is required")]
        public long TestID { get; set; }
        public virtual Test Test { get; set; }

        [Display(Name = "Athlete")]
        [Required(ErrorMessage = "Athlete is required")]
        public int AthleteID { get; set; }
        public virtual ApplicationUser Athlete { get; set; }
        
        [DisplayName("Distance")]
        [Required(ErrorMessage = "This value is required")]
        public int TestValue { get; set; }

        [Display(Name = "FitnessRating")]
        [Required(ErrorMessage = "Fitness Rating is required")]
        public long FitnessRatingID { get; set; }
        public virtual FitnessRating FitnessRating { get; set; }
    }
}
