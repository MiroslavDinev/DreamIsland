namespace DreamIsland.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    public partial class PublicColumnForBusinessModels : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Islands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Collectibles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Celebrities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Islands");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Collectibles");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Celebrities");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                table: "Cars");
        }
    }
}
