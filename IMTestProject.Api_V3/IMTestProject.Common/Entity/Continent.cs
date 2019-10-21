using System;
using System.Collections.Generic;
using IMTestProject.Common.Entity;

namespace IMTestProject.Common
{
    public  class Continent : MultiTenantEntity<int, Guid>
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public virtual ICollection<AdditionalInformation> TableMappingDatas { get; set; } = new HashSet<AdditionalInformation>();
    }
}