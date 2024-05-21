using IMS.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Infrastructure.Entity_Configuration
{
    public class StockInfoConfiguration : IEntityTypeConfiguration<StockInfo>
    {
        public void Configure(EntityTypeBuilder<StockInfo> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();
            
            builder.Property(e => e.Quantity)
               .HasColumnType("float");



            builder.HasOne(e => e.StoreInfo)
                .WithMany(pt => pt.StockInfos)
                .HasForeignKey(e => e.StoreInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CategoryInfo)
                .WithMany(pt => pt.StockInfos)
                .HasForeignKey(e => e.CategoryInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ProductRateInfo)
                .WithMany(pt => pt.StockInfos)
                .HasForeignKey(e => e.ProductRateInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.ProductInfo)
                .WithMany(pt => pt.StockInfos)
                .HasForeignKey(e => e.ProductInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(e => e.IsActive)
                .HasDefaultValue(true);

            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()")
                .HasColumnType("datetime");

            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(true);
            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime");

            builder.Property(e => e.ModifiedBy)
                .IsUnicode(true);
        }
    }
}
