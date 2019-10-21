using System.ComponentModel.DataAnnotations;

namespace IMTestProject.Common.Dto.TableMappingData
{
    public class AddAdditionalInformationDto
    {
        [Required]
        public string Value { get; set; }

        [Required]
        public int ContinentId { get; set; }

        [Required]
        public int TableConfigurationId { get; set; }
    }
}