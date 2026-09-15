using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IMS.web.Migrations.IMSDb
{
    /// <inheritdoc />
    public partial class initclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CustomReportViewModels",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NetAmount = table.Column<double>(type: "float", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    TotalAmount = table.Column<double>(type: "float", nullable: false),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DashboardIndices",
                columns: table => new
                {
                    TotalTransaction = table.Column<double>(type: "float", nullable: false),
                    Sucessful = table.Column<int>(type: "int", nullable: false),
                    Canelled = table.Column<int>(type: "int", nullable: false),
                    Completed = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "DashboardLists",
                columns: table => new
                {
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Count = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "ReportDetailViewModels",
                columns: table => new
                {
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProductName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    UnitName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    SupplierName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "StoreInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    RegistrationNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    PanNo = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UnitInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UnitName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UnitInfo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserInfos",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInfos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CategoryInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CategoryDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    StoreInfoId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CategoryInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CategoryInfo_StoreInfo_StoreInfoId",
                        column: x => x.StoreInfoId,
                        principalTable: "StoreInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CustomerInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreInfoId = table.Column<int>(type: "int", nullable: false),
                    CustomerName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PanNo = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CustomerInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CustomerInfo_StoreInfo_StoreInfoId",
                        column: x => x.StoreInfoId,
                        principalTable: "StoreInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RackInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RackName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    StoreInfoId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RackInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RackInfo_StoreInfo_StoreInfoId",
                        column: x => x.StoreInfoId,
                        principalTable: "StoreInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SupplierInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SupplierName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    StoreInfoId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierInfo_StoreInfo_StoreInfoId",
                        column: x => x.StoreInfoId,
                        principalTable: "StoreInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryInfoId = table.Column<int>(type: "int", nullable: false),
                    ProductName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ProductDescription = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    UnitInfoId = table.Column<int>(type: "int", nullable: false),
                    StoreInfoId = table.Column<int>(type: "int", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInfo_CategoryInfo_CategoryInfoId",
                        column: x => x.CategoryInfoId,
                        principalTable: "CategoryInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductInfo_StoreInfo_StoreInfoId",
                        column: x => x.StoreInfoId,
                        principalTable: "StoreInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductInfo_UnitInfo_UnitInfoId",
                        column: x => x.UnitInfoId,
                        principalTable: "UnitInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductInvoiceInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    InvoiceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StoreInfoId = table.Column<int>(type: "int", nullable: false),
                    CustomerInfoId = table.Column<int>(type: "int", nullable: false),
                    TransactionDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    NetAmount = table.Column<double>(type: "float", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    TotalAmount = table.Column<double>(type: "float", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    BillStatus = table.Column<int>(type: "int", nullable: false),
                    CancellationRemarks = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInvoiceInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInvoiceInfo_CustomerInfo_CustomerInfoId",
                        column: x => x.CustomerInfoId,
                        principalTable: "CustomerInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductInvoiceInfo_StoreInfo_StoreInfoId",
                        column: x => x.StoreInfoId,
                        principalTable: "StoreInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductRateInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryInfoId = table.Column<int>(type: "int", nullable: false),
                    ProductInfoId = table.Column<int>(type: "int", nullable: false),
                    StoreInfoId = table.Column<int>(type: "int", nullable: false),
                    CostPrice = table.Column<double>(type: "float", nullable: false),
                    SellingPrice = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    SoldQuantity = table.Column<double>(type: "float", nullable: false),
                    RemainingQuantity = table.Column<double>(type: "float", nullable: false),
                    BatchNo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PurchasedDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    Expirydate = table.Column<DateTime>(type: "datetime", nullable: true),
                    SupplierInfoId = table.Column<int>(type: "int", nullable: false),
                    RackInfoId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductRateInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductRateInfo_CategoryInfo_CategoryInfoId",
                        column: x => x.CategoryInfoId,
                        principalTable: "CategoryInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductRateInfo_ProductInfo_ProductInfoId",
                        column: x => x.ProductInfoId,
                        principalTable: "ProductInfo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductRateInfo_RackInfo_RackInfoId",
                        column: x => x.RackInfoId,
                        principalTable: "RackInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductRateInfo_StoreInfo_StoreInfoId",
                        column: x => x.StoreInfoId,
                        principalTable: "StoreInfo",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ProductRateInfo_SupplierInfo_SupplierInfoId",
                        column: x => x.SupplierInfoId,
                        principalTable: "SupplierInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductInvoiceDetailInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductInvoiceInfoId = table.Column<int>(type: "int", nullable: false),
                    ProductRateInfoId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductInvoiceDetailInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductInvoiceDetailInfo_ProductInvoiceInfo_ProductInvoiceInfoId",
                        column: x => x.ProductInvoiceInfoId,
                        principalTable: "ProductInvoiceInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductInvoiceDetailInfo_ProductRateInfo_ProductRateInfoId",
                        column: x => x.ProductRateInfoId,
                        principalTable: "ProductRateInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryInfoId = table.Column<int>(type: "int", nullable: false),
                    ProductInfoId = table.Column<int>(type: "int", nullable: false),
                    ProductRateInfoId = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    StoreInfoId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockInfo_CategoryInfo_CategoryInfoId",
                        column: x => x.CategoryInfoId,
                        principalTable: "CategoryInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockInfo_ProductInfo_ProductInfoId",
                        column: x => x.ProductInfoId,
                        principalTable: "ProductInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockInfo_ProductRateInfo_ProductRateInfoId",
                        column: x => x.ProductRateInfoId,
                        principalTable: "ProductRateInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockInfo_StoreInfo_StoreInfoId",
                        column: x => x.StoreInfoId,
                        principalTable: "StoreInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TransactionInfo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    CategoryInfoId = table.Column<int>(type: "int", nullable: false),
                    ProductInfoId = table.Column<int>(type: "int", nullable: false),
                    ProductRateInfoId = table.Column<int>(type: "int", nullable: false),
                    UnitInfoId = table.Column<int>(type: "int", nullable: false),
                    StoreInfoId = table.Column<int>(type: "int", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    Quantity = table.Column<double>(type: "float", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "GETDATE()"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    ModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransactionInfo_CategoryInfo_CategoryInfoId",
                        column: x => x.CategoryInfoId,
                        principalTable: "CategoryInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransactionInfo_ProductInfo_ProductInfoId",
                        column: x => x.ProductInfoId,
                        principalTable: "ProductInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionInfo_ProductRateInfo_ProductRateInfoId",
                        column: x => x.ProductRateInfoId,
                        principalTable: "ProductRateInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionInfo_StoreInfo_StoreInfoId",
                        column: x => x.StoreInfoId,
                        principalTable: "StoreInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransactionInfo_UnitInfo_UnitInfoId",
                        column: x => x.UnitInfoId,
                        principalTable: "UnitInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CategoryInfo_StoreInfoId",
                table: "CategoryInfo",
                column: "StoreInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_CustomerInfo_StoreInfoId",
                table: "CustomerInfo",
                column: "StoreInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInfo_CategoryInfoId",
                table: "ProductInfo",
                column: "CategoryInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInfo_StoreInfoId",
                table: "ProductInfo",
                column: "StoreInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInfo_UnitInfoId",
                table: "ProductInfo",
                column: "UnitInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInvoiceDetailInfo_ProductInvoiceInfoId",
                table: "ProductInvoiceDetailInfo",
                column: "ProductInvoiceInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInvoiceDetailInfo_ProductRateInfoId",
                table: "ProductInvoiceDetailInfo",
                column: "ProductRateInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInvoiceInfo_CustomerInfoId",
                table: "ProductInvoiceInfo",
                column: "CustomerInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductInvoiceInfo_StoreInfoId",
                table: "ProductInvoiceInfo",
                column: "StoreInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRateInfo_CategoryInfoId",
                table: "ProductRateInfo",
                column: "CategoryInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRateInfo_ProductInfoId",
                table: "ProductRateInfo",
                column: "ProductInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRateInfo_RackInfoId",
                table: "ProductRateInfo",
                column: "RackInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRateInfo_StoreInfoId",
                table: "ProductRateInfo",
                column: "StoreInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductRateInfo_SupplierInfoId",
                table: "ProductRateInfo",
                column: "SupplierInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_RackInfo_StoreInfoId",
                table: "RackInfo",
                column: "StoreInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInfo_CategoryInfoId",
                table: "StockInfo",
                column: "CategoryInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInfo_ProductInfoId",
                table: "StockInfo",
                column: "ProductInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInfo_ProductRateInfoId",
                table: "StockInfo",
                column: "ProductRateInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_StockInfo_StoreInfoId",
                table: "StockInfo",
                column: "StoreInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierInfo_StoreInfoId",
                table: "SupplierInfo",
                column: "StoreInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionInfo_CategoryInfoId",
                table: "TransactionInfo",
                column: "CategoryInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionInfo_ProductInfoId",
                table: "TransactionInfo",
                column: "ProductInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionInfo_ProductRateInfoId",
                table: "TransactionInfo",
                column: "ProductRateInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionInfo_StoreInfoId",
                table: "TransactionInfo",
                column: "StoreInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionInfo_UnitInfoId",
                table: "TransactionInfo",
                column: "UnitInfoId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CustomReportViewModels");

            migrationBuilder.DropTable(
                name: "DashboardIndices");

            migrationBuilder.DropTable(
                name: "DashboardLists");

            migrationBuilder.DropTable(
                name: "ProductInvoiceDetailInfo");

            migrationBuilder.DropTable(
                name: "ReportDetailViewModels");

            migrationBuilder.DropTable(
                name: "StockInfo");

            migrationBuilder.DropTable(
                name: "TransactionInfo");

            migrationBuilder.DropTable(
                name: "UserInfos");

            migrationBuilder.DropTable(
                name: "ProductInvoiceInfo");

            migrationBuilder.DropTable(
                name: "ProductRateInfo");

            migrationBuilder.DropTable(
                name: "CustomerInfo");

            migrationBuilder.DropTable(
                name: "ProductInfo");

            migrationBuilder.DropTable(
                name: "RackInfo");

            migrationBuilder.DropTable(
                name: "SupplierInfo");

            migrationBuilder.DropTable(
                name: "CategoryInfo");

            migrationBuilder.DropTable(
                name: "UnitInfo");

            migrationBuilder.DropTable(
                name: "StoreInfo");
        }
    }
}
