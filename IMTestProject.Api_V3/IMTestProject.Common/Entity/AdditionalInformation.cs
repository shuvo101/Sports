using System;
using System.Collections.Generic;
using System.Text;

namespace IMTestProject.Common.Entity
{
   public class AdditionalInformation : MultiTenantEntity<int, Guid>
    {
        public string Value { get; set; }

        public string Code { get; set; }

        public int ContinentId { get; set; }
        public virtual Continent Continent { get; set; }

        public int TableConfigurationId { get; set; }
        public virtual TableConfiguration TableConfiguration { get; set; }

    }
}
