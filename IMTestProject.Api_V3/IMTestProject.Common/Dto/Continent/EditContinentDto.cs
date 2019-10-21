using System.ComponentModel.DataAnnotations;

namespace IMTestProject.Common.Dto.Continent
{
    public class EditContinentDto
    {
        [Required]
        [MaxLength (4)]
        [StringLength (4)]
        public string Code { get; set; }
        
        [Required]
        [MaxLength (256)]
        [StringLength (256)]
        public string Name { get; set; }
        
        [Required]
        public int  Id { get; set; }
    }
}