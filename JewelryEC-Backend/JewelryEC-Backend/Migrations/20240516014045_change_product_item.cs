using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class change_product_item : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProductImages");

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Created_at",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "InternationalCode",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Updated_at",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "State",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "Stock",
                table: "ProductItems");

            migrationBuilder.DropColumn(
                name: "Created_at",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Catalogs");

            migrationBuilder.DropColumn(
                name: "Updated_at",
                table: "Catalogs");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                table: "ProductItems",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "Size",
                table: "ProductItems",
                newName: "Tags");

            migrationBuilder.RenameColumn(
                name: "SKU",
                table: "ProductItems",
                newName: "Image");

            migrationBuilder.RenameColumn(
                name: "ProductSlug",
                table: "ProductItems",
                newName: "Description");

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPrice",
                table: "ProductItems",
                type: "numeric",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<double>(
                name: "DiscountPercent",
                table: "ProductItems",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");
            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductItems");

            //migrationBuilder.AlterColumn<int>(
            //    name: "Id",
            //    table: "ProductItems",
            //    type: "integer",
            //    nullable: false,
            //    oldClrType: typeof(Guid),
            //    oldType: "uuid")
            //    .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Tags",
                table: "ProductItems",
                newName: "Size");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "ProductItems",
                newName: "UnitPrice");

            migrationBuilder.RenameColumn(
                name: "Image",
                table: "ProductItems",
                newName: "SKU");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "ProductItems",
                newName: "ProductSlug");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_at",
                table: "Products",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "InternationalCode",
                table: "Products",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_at",
                table: "Products",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountPrice",
                table: "ProductItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "numeric",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DiscountPercent",
                table: "ProductItems",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "Id",
            //    table: "ProductItems",
            //    type: "uuid",
            //    nullable: false,
            //    oldClrType: typeof(int),
            //    oldType: "integer")
            //    .OldAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<int>(
                name: "State",
                table: "ProductItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Stock",
                table: "ProductItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created_at",
                table: "Catalogs",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Catalogs",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated_at",
                table: "Catalogs",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ProductImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductImages_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProductImages_ProductId",
                table: "ProductImages",
                column: "ProductId");
        }
    }
}
