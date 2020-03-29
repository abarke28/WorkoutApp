using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutApp.Migrations
{
    public partial class RedoExerciseStationMappingManyToMany : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Station_StationId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_StationId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Exercises");

            migrationBuilder.CreateTable(
                name: "ExerciseStation",
                columns: table => new
                {
                    ExerciseId = table.Column<int>(nullable: false),
                    StationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExerciseStation", x => new { x.ExerciseId, x.StationId });
                    table.ForeignKey(
                        name: "FK_ExerciseStation_Exercises_ExerciseId",
                        column: x => x.ExerciseId,
                        principalTable: "Exercises",
                        principalColumn: "ExerciseId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExerciseStation_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Station",
                        principalColumn: "StationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExerciseStation_StationId",
                table: "ExerciseStation",
                column: "StationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExerciseStation");

            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "Exercises",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_StationId",
                table: "Exercises",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Station_StationId",
                table: "Exercises",
                column: "StationId",
                principalTable: "Station",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
