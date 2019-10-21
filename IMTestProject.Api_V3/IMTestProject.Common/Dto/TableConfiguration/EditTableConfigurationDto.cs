using System.ComponentModel.DataAnnotations;

namespace IMTestProject.Common.Dto.TableConfiguration
{
    public class EditTableConfigurationDto
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string DisplayName { get; set; }

        [Required]
        public string ControlType { get; set; }

        [Required]
        public string DataType { get; set; }

        
        public string ReferanceTable { get; set; }

        
        public string TextField { get; set; }

        
        public string ValueField { get; set; }

        
        public int  Id { get; set; }
    }
}