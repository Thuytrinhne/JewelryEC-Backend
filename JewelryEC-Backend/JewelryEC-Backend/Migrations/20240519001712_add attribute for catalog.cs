using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class addattributeforcatalog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Catalogs",
                type: "text",
                nullable: true,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Catalogs",
                type: "text",
                nullable: true,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Catalogs");
        }
    }
}
