using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JewelryEC_Backend.Migrations
{
    /// <inheritdoc />
    public partial class retype_productVariant_id : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "NewId",
                table: "ProductVariants",
                type: "uuid",
                nullable: false,
                defaultValueSql: "uuid_generate_v4()");

            // Step 2: Copy data from old Id to NewId (if necessary)
            // If Id mapping is needed, it could be done here. 
            // Assuming there's no need to map old Id to new Id:
            // This step is omitted as the new Ids will be generated.

            // Step 3: Drop foreign key constraints referencing the old Id column
            migrationBuilder.DropForeignKey(
                name: "FK_ProductVariants_Products_ProductId",
                table: "ProductVariants");

            // Step 4: Drop the old Id column
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProductVariants",
                table: "ProductVariants");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ProductVariants");

            // Step 5: Rename NewId to Id
            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "ProductVariants",
                newName: "Id");

            // Step 6: Add primary key on the new Id column
            migrationBuilder.AddPrimaryKey(
                name: "PK_ProductVariants",
                table: "ProductVariants",
                column: "Id");

            // Step 7: Re-add foreign key constraints referencing the new Id column
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
        }
    }
}
