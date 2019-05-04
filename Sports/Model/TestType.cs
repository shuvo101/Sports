using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Sports.API.Model
{
    [Table("TestType")]
    public class TestType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long ID { get; set; }

        [StringLength(50)]
        [DisplayName("Type Name")]
        [Required(ErrorMessage = "Test type name is required")]
        public string Name { get; set; }

        public virtual List<Test> Tests { get; set; }
        public virtual List<FitnessRating> FitnessRatings { get; set; }
    }
}
