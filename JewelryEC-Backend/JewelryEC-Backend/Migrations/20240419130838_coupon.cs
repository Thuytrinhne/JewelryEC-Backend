using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class coupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CatalogCoupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CatalogId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountValue = table.Column<double>(type: "double precision", nullable: false),
                    DiscountUnit = table.Column<int>(type: "integer", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CouponCode = table.Column<string>(type: "text", nullable: false),
                    MinimumOrderValue = table.Column<decimal>(type: "numeric", nullable: true),
                    MaximumDiscountValue = table.Column<decimal>(type: "numeric", nullable: true),
                    IsRedeemAllowed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CatalogCoupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CatalogCoupons_Catalogs_CatalogId",
                        column: x => x.CatalogId,
                        principalTable: "Catalogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductCoupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    DiscountValue = table.Column<double>(type: "double precision", nullable: false),
                    DiscountUnit = table.Column<int>(type: "integer", nullable: false),
                    CreatedTime = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ValidFrom = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ValidTo = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    CouponCode = table.Column<string>(type: "text", nullable: false),
                    MinimumOrderValue = table.Column<decimal>(type: "numeric", nullable: true),
                    MaximumDiscountValue = table.Column<decimal>(type: "numeric", nullable: true),
                    IsRedeemAllowed = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCoupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCoupons_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CatalogCoupons_CatalogId",
                table: "CatalogCoupons",
                column: "CatalogId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCoupons_ProductId",
                table: "ProductCoupons",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CatalogCoupons");

            migrationBuilder.DropTable(
                name: "ProductCoupons");
        }
    }
}
