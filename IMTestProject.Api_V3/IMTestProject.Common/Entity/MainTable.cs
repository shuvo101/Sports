using System;
using System.Collections.Generic;
using System.Text;

namespace IMTestProject.Common.Entity
{
   public class MainTable : MultiTenantEntity<int, Guid>
    {
        public string TableName { get; set; }
        public string TableCode { get; set; }
        //public virtual ICollection<TableConfiguration> TableConfigurations { get; set; } = new HashSet<TableConfiguration>();
        //public virtual ICollection<AdditionalInformation> TableMappingDatas { get; set; } = new HashSet<AdditionalInformation>();
    }
}
