using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class editProductItem_state : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

           
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Catalogs",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Catalogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "Catalogs",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_at",
                table: "Catalogs",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "State",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "Updated_at",
                table: "Catalogs");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Catalogs",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");
         
        }
    }
}
