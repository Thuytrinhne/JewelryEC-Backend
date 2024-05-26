using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class CreateRelationshipBetweenOrderItemAndUserCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserCouponId",
                table: "OrderItems",
                type: "uuid",
                nullable: true
                );

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_UserCouponId",
                table: "OrderItems",
                column: "UserCouponId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_UserCoupons_UserCouponId",
                table: "OrderItems",
                column: "UserCouponId",
                principalTable: "UserCoupons",
                principalColumn: "Id"
                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_UserCoupons_UserCouponId",
                table: "OrderItems");

            migrationBuilder.DropIndex(
                name: "IX_OrderItems_UserCouponId",
                table: "OrderItems");

            migrationBuilder.DropColumn(
                name: "UserCouponId",
                table: "OrderItems");
        }
    }
}
