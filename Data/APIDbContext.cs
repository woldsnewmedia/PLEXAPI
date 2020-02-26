using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PLEXAPI.Models;
using PLEXAPI.Models.Account;

namespace PLEXAPI.Data
{
    public class APIDbContext : DbContext
    {

        public APIDbContext(DbContextOptions<APIDbContext> options)
            : base(options)
        {
        }

        public DbSet<APIUser> APIUser { get; set; }

    }
}
