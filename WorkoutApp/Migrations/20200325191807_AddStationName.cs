using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutApp.Migrations
{
    public partial class AddStationName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StationName",
                table: "Station",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StationName",
                table: "Station");
        }
    }
}
