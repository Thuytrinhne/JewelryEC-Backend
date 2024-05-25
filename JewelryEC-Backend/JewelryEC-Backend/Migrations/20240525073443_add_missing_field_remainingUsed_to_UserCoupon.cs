using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class add_missing_field_remainingUsed_to_UserCoupon : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RemainingUsage",
                table: "UserCoupons",
                type: "integer",
                nullable: false,
                defaultValue: 0);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainingUsage",
                table: "UserCoupons");

        }
    }
}
