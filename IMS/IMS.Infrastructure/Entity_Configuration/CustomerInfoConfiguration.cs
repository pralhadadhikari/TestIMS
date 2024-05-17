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
    public class CustomerInfoConfiguration : IEntityTypeConfiguration<CustomerInfo>
    {
        public void Configure(EntityTypeBuilder<CustomerInfo> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();
            builder.Property(e => e.CustomerName)
                .HasMaxLength(200)
                .IsUnicode(true);
            builder.Property(e => e.Email)
                .HasMaxLength(200)
                .IsUnicode(true);
            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(200)
                .IsUnicode(true);

            builder.Property(e => e.Address)
               .HasMaxLength(200)
               .IsUnicode(true);

            builder.Property(e => e.PanNo)
              .HasMaxLength(200)
              .IsUnicode(true);

            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasDefaultValueSql("GETDATE()");
            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .IsUnicode(true);

            builder.HasOne(e => e.StoreInfo)
            .WithMany(pt => pt.CustomerInfos)
            .HasForeignKey(e => e.StoreInfoId);

            builder.HasMany(e => e.ProductInvoiceInfos)
            .WithOne(pt => pt.CustomerInfo)
            .HasForeignKey(e => e.CustomerInfoId);


        }
    }
}
