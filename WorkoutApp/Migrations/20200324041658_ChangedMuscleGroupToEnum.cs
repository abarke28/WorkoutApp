using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutApp.Migrations
{
    public partial class ChangedMuscleGroupToEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MuscleGroup",
                table: "Exercises");

            migrationBuilder.AddColumn<int>(
                name: "ExerciseType",
                table: "Exercises",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExerciseType",
                table: "Exercises");

            migrationBuilder.AddColumn<string>(
                name: "MuscleGroup",
                table: "Exercises",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
