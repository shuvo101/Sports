using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Runtime.Serialization;

namespace IMTestProject.Common.Entity
{
    public abstract class MultiTenantEntity<TPk, TTenant> : AuditEntity<TPk>,IMultiTenantEntity<TTenant> 
    {
        [Column("TenantId", Order = 1)]
        public TTenant TenantId { get; set; }
    }
}
