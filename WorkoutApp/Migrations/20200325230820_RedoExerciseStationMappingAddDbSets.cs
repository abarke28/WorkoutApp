using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutApp.Migrations
{
    public partial class RedoExerciseStationMappingAddDbSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseStation_Exercises_ExerciseId",
                table: "ExerciseStation");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseStation_Station_StationId",
                table: "ExerciseStation");

            migrationBuilder.DropForeignKey(
                name: "FK_Station_Workouts_WorkoutId",
                table: "Station");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Station",
                table: "Station");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseStation",
                table: "ExerciseStation");

            migrationBuilder.RenameTable(
                name: "Station",
                newName: "Stations");

            migrationBuilder.RenameTable(
                name: "ExerciseStation",
                newName: "ExerciseStations");

            migrationBuilder.RenameIndex(
                name: "IX_Station_WorkoutId",
                table: "Stations",
                newName: "IX_Stations_WorkoutId");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseStation_StationId",
                table: "ExerciseStations",
                newName: "IX_ExerciseStations_StationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stations",
                table: "Stations",
                column: "StationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseStations",
                table: "ExerciseStations",
                columns: new[] { "ExerciseId", "StationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseStations_Exercises_ExerciseId",
                table: "ExerciseStations",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "ExerciseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseStations_Stations_StationId",
                table: "ExerciseStations",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stations_Workouts_WorkoutId",
                table: "Stations",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "WorkoutId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseStations_Exercises_ExerciseId",
                table: "ExerciseStations");

            migrationBuilder.DropForeignKey(
                name: "FK_ExerciseStations_Stations_StationId",
                table: "ExerciseStations");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Workouts_WorkoutId",
                table: "Stations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stations",
                table: "Stations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ExerciseStations",
                table: "ExerciseStations");

            migrationBuilder.RenameTable(
                name: "Stations",
                newName: "Station");

            migrationBuilder.RenameTable(
                name: "ExerciseStations",
                newName: "ExerciseStation");

            migrationBuilder.RenameIndex(
                name: "IX_Stations_WorkoutId",
                table: "Station",
                newName: "IX_Station_WorkoutId");

            migrationBuilder.RenameIndex(
                name: "IX_ExerciseStations_StationId",
                table: "ExerciseStation",
                newName: "IX_ExerciseStation_StationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Station",
                table: "Station",
                column: "StationId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ExerciseStation",
                table: "ExerciseStation",
                columns: new[] { "ExerciseId", "StationId" });

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseStation_Exercises_ExerciseId",
                table: "ExerciseStation",
                column: "ExerciseId",
                principalTable: "Exercises",
                principalColumn: "ExerciseId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExerciseStation_Station_StationId",
                table: "ExerciseStation",
                column: "StationId",
                principalTable: "Station",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Station_Workouts_WorkoutId",
                table: "Station",
                column: "WorkoutId",
                principalTable: "Workouts",
                principalColumn: "WorkoutId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
