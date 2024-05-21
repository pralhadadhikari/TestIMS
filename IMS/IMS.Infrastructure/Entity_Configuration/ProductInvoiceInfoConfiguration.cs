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
    public class ProductInvoiceInfoConfiguration : IEntityTypeConfiguration<ProductInvoiceInfo>
    {
        public void Configure(EntityTypeBuilder<ProductInvoiceInfo> builder)
        {
            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
               .ValueGeneratedOnAdd();
            builder.Property(e => e.PaymentMethod)
                .HasColumnType("int");

            builder.Property(e => e.TransactionDate)
                .HasColumnType("datetime");

            builder.Property(e => e.NetAmount)
                .HasColumnType("float");

            builder.Property(e => e.DiscountAmount)
                .HasColumnType("float");

            builder.Property(e => e.TotalAmount)
                .HasColumnType("float");

            builder.Property(e => e.Remarks)
                .HasMaxLength(200)
                .IsUnicode(true);

            builder.Property(e => e.BillStatus)
                .HasColumnType("int");

            builder.Property(e => e.CancellationRemarks)
                .HasMaxLength(200)
                .IsUnicode(true);

            builder.HasOne(e => e.StoreInfo)
                .WithMany(pt => pt.ProductInvoiceInfos)
                .HasForeignKey(e => e.StoreInfoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(e => e.CustomerInfo)
            .WithMany(pt => pt.ProductInvoiceInfos)
            .HasForeignKey(e => e.CustomerInfoId)
            .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(e => e.ProductInvoiceDetailInfos)
            .WithOne(pt => pt.ProductInvoiceInfo)
            .HasForeignKey(e => e.ProductInvoiceInfoId);


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
        }
    }
}
