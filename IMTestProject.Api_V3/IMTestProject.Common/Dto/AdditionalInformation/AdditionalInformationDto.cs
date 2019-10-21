using IMTestProject.Common.Dto.TableConfiguration;
using System.ComponentModel.DataAnnotations;

namespace IMTestProject.Common.Dto.TableMappingData
{
    public class AdditionalInformationDto
    {
        [Required]
        public string Value { get; set; }

        
        public int ContinentId { get; set; }

        [Required]
        public int TableConfigurationId { get; set; }

        public int  Id { get; set; }

        public TableConfigurationDto TableConfigurationDtos { get; set; }
    }
}