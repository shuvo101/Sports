using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.API.Model
{
    [Table("CoachAthlete")]
    public class CoachAthlete : BaseModel
    {
        [Display(Name = "Coach")]
        [Required(ErrorMessage = "Coach is required")]
        public int CoachID { get; set; }
        public virtual ApplicationUser Coach { get; set; }

        [Display(Name = "Athlete")]
        [Required(ErrorMessage = "Athlete is required")]
        public int AthleteID { get; set; }
        public virtual ApplicationUser Athlete { get; set; }
    }
}
