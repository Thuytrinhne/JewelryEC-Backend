using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class add_product_variants : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ProductItems",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", Npgsql.EntityFrameworkCore.PostgreSQL.Metadata.NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            // Rename the table
            migrationBuilder.RenameTable(
                name: "ProductItems",
                newName: "ProductVariants");

            // Rename the index
            migrationBuilder.RenameIndex(
                name: "IX_ProductItems_ProductId",
                table: "ProductVariants",
                newName: "IX_ProductVariants_ProductId");

            // Add the new primary key constraint on the Id column
            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariants",
                table: "ProductVariants",
                column: "Id");

            // Add the foreign key constraint
            migrationBuilder.AddForeignKey(
                name: "FK_ProductVariants_Products_ProductId",
                table: "ProductVariants",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Products_ProductId",
                table: "ProductVariants");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariants",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductVariants");

            migrationBuilder.RenameTable(
                name: "ProductVariants",
                newName: "ProductItems");

            migrationBuilder.RenameIndex(
                name: "IX_ProductVariants_ProductId",
                table: "ProductItems",
                newName: "IX_ProductItems_ProductId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductItems",
                table: "ProductItems",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductItems_Products_ProductId",
                table: "ProductItems",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
