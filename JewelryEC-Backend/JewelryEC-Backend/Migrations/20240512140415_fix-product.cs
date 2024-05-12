using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class fixproduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Availability",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SaledCount",
                table: "Products");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "ProductItems",
                newName: "Size");

            //migrationBuilder.AlterColumn<int>(
            //    name: "State",
            //    table: "ProductItems",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(string),
            //    oldType: "text");

            migrationBuilder.AddColumn<int>(
                name: "PaymentMethod",
                table: "Orders",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PaymentMethod",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "ProductItems",
                newName: "Name");

            migrationBuilder.AddColumn<double>(
                name: "Availability",
                table: "Products",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<long>(
                name: "SaledCount",
                table: "Products",
                type: "bigint",
                nullable: true);

            //migrationBuilder.AlterColumn<string>(
            //    name: "State",
            //    table: "ProductItems",
            //    type: "text",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "integer");
        }
    }
}
