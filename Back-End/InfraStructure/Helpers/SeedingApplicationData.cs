using InfraStructure.Helpers.Seeding;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InfraStructure.Helpers
{
    public static class SeedingApplicationData
    {
        public static void SeedAppData(this ModelBuilder model)
        {
            model.SeedRoles();
            model.SeedAdmins();
            model.SeedJourneys();
        }
    }
}
