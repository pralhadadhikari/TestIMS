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
    public class StoreConfiguration : IEntityTypeConfiguration<StoreInfo>
    {
        public void Configure(EntityTypeBuilder<StoreInfo> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();

            builder.Property(e => e.StoreName)
                .HasMaxLength(200)
                .IsUnicode(true);

            builder.Property(e => e.Address)
                .HasMaxLength(200)
                .IsUnicode(true);
            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(true);
            builder.Property(e => e.RegistrationNo)
                .HasMaxLength(30)
                .IsUnicode(true);
            builder.Property(e => e.PanNo)
                .HasMaxLength(30)
               .IsUnicode(true);
            builder.Property(e => e.LogoUrl)
                .HasMaxLength(500)
                .IsUnicode(true);
            builder.Property(e => e.IsActive)
            .HasDefaultValue(true);

            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(true);
            builder.Property(e => e.ModifiedDate)
                .HasColumnType("datetime");               
               
            builder.Property(e => e.ModifiedBy)                
                .IsUnicode(true);

            builder.HasMany(e => e.CategoryInfos)
                .WithOne(pt => pt.StoreInfo)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.CustomerInfos)
                .WithOne(pt => pt.StoreInfo)
                .HasForeignKey(e => e.StoreInfoId);
            builder.HasMany(e => e.ProductInfos)
                .WithOne(pt => pt.StoreInfo)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.ProductInvoiceInfos)
                .WithOne(pt => pt.StoreInfo)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.ProductRateInfos)
                .WithOne(pt => pt.StoreInfo)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.RackInfos)
                .WithOne(pt => pt.StoreInfo)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.StockInfos)
                .WithOne(pt => pt.StoreInfo)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.SupplierInfos)
                .WithOne(pt => pt.StoreInfo)
                .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.TransactionInfos)
                .WithOne(pt => pt.StoreInfo)
                .HasForeignKey(e => e.StoreInfoId);
        }
    }
}
