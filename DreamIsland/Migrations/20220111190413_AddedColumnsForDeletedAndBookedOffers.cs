namespace DreamIsland.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    public partial class AddedColumnsForDeletedAndBookedOffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "Islands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Islands",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "Collectibles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Collectibles",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "Celebrities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Celebrities",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsBooked",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Cars",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "Islands");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Islands");

            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "Collectibles");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Collectibles");

            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "Celebrities");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Celebrities");

            migrationBuilder.DropColumn(
                name: "IsBooked",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Cars");
        }
    }
}
