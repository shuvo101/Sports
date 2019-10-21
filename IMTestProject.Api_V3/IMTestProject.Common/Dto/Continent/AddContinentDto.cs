using IMTestProject.Common.Dto.TableConfiguration;
using IMTestProject.Common.Dto.TableMappingData;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IMTestProject.Common.Dto.Continent
{
    public class AddContinentDto
    {
        [Required]
        [MaxLength (4)]
        [StringLength (4)]
        public string Code { get; set; }
        
        [Required]
        [MaxLength (256)]
        [StringLength (256)]
        public string Name { get; set; }

        public List<TableConfigurationDto> TableConfigurationDtos { get; set; }
    }
}