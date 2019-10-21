using System.ComponentModel.DataAnnotations;

namespace IMTestProject.Common.Dto.TableConfiguration
{
    public class AddTableConfigurationDto
    {
        [Required]
        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string ControlType { get; set; }

        [Required]
        public string DataType { get; set; }

        
        public string ReferanceTable { get; set; }

        
        public string TextField { get; set; }

        
        public string ValueField { get; set; }
    }
}