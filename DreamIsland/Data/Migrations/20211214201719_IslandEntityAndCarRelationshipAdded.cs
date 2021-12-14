namespace DreamIsland.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;
    public partial class IslandEntityAndCarRelationshipAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "IslandId",
                table: "Cars",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Islands",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SizeInSquareKm = table.Column<double>(type: "float", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Islands", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cars_IslandId",
                table: "Cars",
                column: "IslandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Cars_Islands_IslandId",
                table: "Cars",
                column: "IslandId",
                principalTable: "Islands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Cars_Islands_IslandId",
                table: "Cars");

            migrationBuilder.DropTable(
                name: "Islands");

            migrationBuilder.DropIndex(
                name: "IX_Cars_IslandId",
                table: "Cars");

            migrationBuilder.DropColumn(
                name: "IslandId",
                table: "Cars");
        }
    }
}
