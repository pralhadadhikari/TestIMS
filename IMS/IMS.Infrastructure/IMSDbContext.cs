using IMS.Infrastructure.Entity_Configuration;
using IMS.Models.Entity;
using IMS.Models.ViewModels;
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
        public DbSet<CustomReportViewModel> CustomReportViewModels { get; set; }       
        public DbSet<ReportDetailViewModel> ReportDetailViewModels { get; set; }
        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<DashboardList> DashboardLists { get; set; }
        public DbSet<DashboardIndex> DashboardIndices { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new StoreConfiguration());
            builder.ApplyConfiguration(new CategoryInfoConfiguration());
            builder.ApplyConfiguration(new CustomerInfoConfiguration());
            builder.ApplyConfiguration(new UnitInfoConfiguration());
            builder.ApplyConfiguration(new ProductInfoConfiguration());
            builder.ApplyConfiguration(new RackInfoConfiguration());
            builder.ApplyConfiguration(new SupplierInfoConfiguration());
            builder.ApplyConfiguration(new ProductRateInfoConfiguration());
            builder.ApplyConfiguration(new StockInfoConfiguration());
            builder.ApplyConfiguration(new TransactionInfoConfiguration());
            builder.ApplyConfiguration(new ProductInvoiceInfoConfiguration());
            builder.ApplyConfiguration(new ProductInvoiceDetailInfoConfiguration());
            builder.Entity<CustomReportViewModel>().HasNoKey();
            builder.Entity<ReportDetailViewModel>().HasNoKey();
            builder.Entity<DashboardList>().HasNoKey();
            builder.Entity<DashboardIndex>().HasNoKey();
        }


    }
}
