using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutApp.Migrations
{
    public partial class AddStationDbSet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Station_StationId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Station_Workouts_WorkoutId",
                table: "Station");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Station",
                table: "Station");

            migrationBuilder.RenameTable(
                name: "Station",
                newName: "Stations");

            migrationBuilder.RenameIndex(
                name: "IX_Station_WorkoutId",
                table: "Stations",
                newName: "IX_Stations_WorkoutId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Stations",
                table: "Stations",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Stations_StationId",
                table: "Exercises",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Exercises_Stations_StationId",
                table: "Exercises");

            migrationBuilder.DropForeignKey(
                name: "FK_Stations_Workouts_WorkoutId",
                table: "Stations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Stations",
                table: "Stations");

            migrationBuilder.RenameTable(
                name: "Stations",
                newName: "Station");

            migrationBuilder.RenameIndex(
                name: "IX_Stations_WorkoutId",
                table: "Station",
                newName: "IX_Station_WorkoutId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Station",
                table: "Station",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Station_StationId",
                table: "Exercises",
                column: "StationId",
                principalTable: "Station",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Restrict);

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
