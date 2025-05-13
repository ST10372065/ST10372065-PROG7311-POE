using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ST10372065_PROG7311.Migrations
{
    /// <inheritdoc />
    public partial class AddPriceAndImageUrlToProduct : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Product",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Product",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Product");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Product");
        }
    }
}
