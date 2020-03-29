using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutApp.Migrations
{
    public partial class AddDescriptionToWorkout : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WorkoutName",
                table: "Workouts");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Workouts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Workouts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Workouts");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Workouts");

            migrationBuilder.AddColumn<string>(
                name: "WorkoutName",
                table: "Workouts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
