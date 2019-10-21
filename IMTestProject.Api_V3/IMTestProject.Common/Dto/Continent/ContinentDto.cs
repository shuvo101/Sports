using IMTestProject.Common.Dto.TableConfiguration;
using IMTestProject.Common.Dto.TableMappingData;
using System.Collections.Generic;

namespace IMTestProject.Common.Dto.Continent
{
    public class ContinentDto
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public int  Id { get; set; }
        public List<AdditionalInformationDto> AdditionalInformationDtos = new List<AdditionalInformationDto>();
    }
}