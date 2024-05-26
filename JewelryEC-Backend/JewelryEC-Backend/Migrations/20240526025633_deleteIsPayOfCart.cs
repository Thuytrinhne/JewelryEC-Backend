using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class deleteIsPayOfCart : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPayed",
                table: "Carts");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IsPayed",
                table: "Carts",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
