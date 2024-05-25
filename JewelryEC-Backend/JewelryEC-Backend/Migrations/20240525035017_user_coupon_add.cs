using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class user_coupon_add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Conditions",
                table: "ProductCoupons",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "ProductCoupons",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "UserCoupons",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductCouponId = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCoupons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCoupons_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserCoupons_ProductCoupons_ProductCouponId",
                        column: x => x.ProductCouponId,
                        principalTable: "ProductCoupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CouponApplications",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserCouponId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CouponApplications", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CouponApplications_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CouponApplications_UserCoupons_UserCouponId",
                        column: x => x.UserCouponId,
                        principalTable: "UserCoupons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CouponApplications_OrderId",
                table: "CouponApplications",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponApplications_UserCouponId",
                table: "CouponApplications",
                column: "UserCouponId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCoupons_ProductCouponId",
                table: "UserCoupons",
                column: "ProductCouponId");

            migrationBuilder.CreateIndex(
                name: "IX_UserCoupons_UserId",
                table: "UserCoupons",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CouponApplications");

            migrationBuilder.DropTable(
                name: "UserCoupons");

            migrationBuilder.DropColumn(
                name: "Conditions",
                table: "ProductCoupons");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "ProductCoupons");
        }
    }
}
