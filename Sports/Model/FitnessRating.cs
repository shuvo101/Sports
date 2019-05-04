using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.API.Model
{
    [Table("FitnessRating")]
    public class FitnessRating
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [StringLength(50)]
        [DisplayName("Type Name")]
        [Required(ErrorMessage = "Test type name is required")]
        public string Name { get; set; }
        
        [DisplayName("Min Value")]
        [Required(ErrorMessage = "Min Value is required")]
        public int MinValue { get; set; }

        [DisplayName("Max Value")]
        public int MaxValue { get; set; }

        [Display(Name = "Test Type")]
        [Required(ErrorMessage = "Test Type is required")]
        public long TestTypeID { get; set; }
        public virtual TestType TestType { get; set; }

        public virtual List<TestAthlete> TestAthletes { get; set; }
    }
}
