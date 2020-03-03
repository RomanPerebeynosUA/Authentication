using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentetyExample.Data
{
    // IdenityDbContext contains all user tables
    public class ApDbContext : IdentityDbContext
    {
        public ApDbContext(DbContextOptions<ApDbContext> options)  
            : base(options)
        {

        }
    }
}
