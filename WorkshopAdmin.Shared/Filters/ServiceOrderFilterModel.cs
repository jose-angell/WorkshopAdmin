using System;
using System.Collections.Generic;
using System.Text;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Filters
{
    public class ServiceOrderFilterModel
    {
        public string Search { get; set; } = string.Empty;

        public bool? IsActive { get; set; }

        public ServiceOrderStatus? orderStatus { get; set; }
        public ServiceType? serviceType { get; set; }
    }
}
