using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class add_coupon_to_orderItem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponApplications_Orders_OrderId",
                table: "CouponApplications");

            migrationBuilder.DropIndex(
                name: "IX_CouponApplications_OrderId",
                table: "CouponApplications");

            migrationBuilder.RenameColumn(
                name: "OrderId",
                table: "CouponApplications",
                newName: "OrderItemId");

            migrationBuilder.AddColumn<Guid>(
                name: "CouponApplicationId",
                table: "OrderItems",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "UserCouponId",
                table: "CartItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_CouponApplications_OrderItemId",
                table: "CouponApplications",
                column: "OrderItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CouponApplications_OrderItems_OrderItemId",
                table: "CouponApplications",
                column: "OrderItemId",
                principalTable: "OrderItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CouponApplications_OrderItems_OrderItemId",
                table: "CouponApplications");

            migrationBuilder.DropIndex(
                name: "IX_CouponApplications_OrderItemId",
                table: "CouponApplications");

            migrationBuilder.DropColumn(
                name: "CouponApplicationId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "UserCouponId",
                table: "CartItems");

            migrationBuilder.RenameColumn(
                name: "OrderItemId",
                table: "CouponApplications",
                newName: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_CouponApplications_OrderId",
                table: "CouponApplications",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_CouponApplications_Orders_OrderId",
                table: "CouponApplications",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
