using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutApp.Migrations
{
    public partial class AddTimePropertiesToWorkout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RepSeconds",
                table: "Workouts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RestSeconds",
                table: "Workouts",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RepSeconds",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "RestSeconds",
                table: "Workouts");
        }
    }
}
