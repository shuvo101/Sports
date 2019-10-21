using System;
using System.Collections.Generic;
using System.Text;

namespace IMTestProject.Common.Entity
{
    public interface IMultiTenantEntity<TTenant>
    {
        TTenant TenantId { get; set; }
    }
}
