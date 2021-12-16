using Microsoft.EntityFrameworkCore.Migrations;

namespace DreamIsland.Data.Migrations
{
    public partial class AddedImageColumnToCelebritiesAndCollectiblesTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Collectibles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Celebrities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Collectibles");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Celebrities");
        }
    }
}
