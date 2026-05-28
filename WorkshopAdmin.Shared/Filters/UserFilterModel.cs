using System;
using System.Collections.Generic;
using System.Text;
using WorkshopAdmin.Shared.Emuns;
using WorkshopAdmin.Shared.Enums;

namespace WorkshopAdmin.Shared.Filters
{
    public class UserFilterModel
    {
        public string Search { get; set; } = string.Empty;

        public bool? IsActive { get; set; }

        public UserRole? role { get; set; }
    }
}
