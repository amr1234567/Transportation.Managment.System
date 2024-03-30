using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public enum Roles
    {
        [Description("User")]
        User,
        [Description("Admin")]
        Admin,
        [Description("BusStopManager")]
        BusStopManager
    }
}
