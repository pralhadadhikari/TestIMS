using IMS.Infrastructure.Entity_Configuration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure
{
    public class IMSDbContext: DbContext
    {
        public IMSDbContext(DbContextOptions<IMSDbContext> Options)
            : base(Options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StoreConfiguration());

        }


    }
}
