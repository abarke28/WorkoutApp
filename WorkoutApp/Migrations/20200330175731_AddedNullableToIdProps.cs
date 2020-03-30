using Microsoft.EntityFrameworkCore.Migrations;

namespace WorkoutApp.Migrations
{
    public partial class AddedNullableToIdProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StationId",
                table: "Exercises",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Exercises_StationId",
                table: "Exercises",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Exercises_Stations_StationId",
                table: "Exercises",
                column: "StationId",
                principalTable: "Stations",
                principalColumn: "StationId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Exercises_Stations_StationId",
                table: "Exercises");

            migrationBuilder.DropIndex(
                name: "IX_Exercises_StationId",
                table: "Exercises");

            migrationBuilder.DropColumn(
                name: "StationId",
                table: "Exercises");
        }
    }
}
