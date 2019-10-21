using System;
using System.Collections.Generic;
using System.Text;

namespace IMTestProject.Common.Entity
{
    public class TableConfiguration : MultiTenantEntity<int, Guid>
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ControlType { get; set; }
        public string DataType { get; set; }
        public string ReferanceTable { get; set; }
        public string TextField { get; set; }
        public string ValueField { get; set; }
        public virtual ICollection<AdditionalInformation> AdditionalInformations { get; set; } = new HashSet<AdditionalInformation>();
    }
}
