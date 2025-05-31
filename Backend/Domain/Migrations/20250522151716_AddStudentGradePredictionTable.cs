using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Backend.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentGradePredictionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ParticipationPoints",
                table: "Students");

            migrationBuilder.AddColumn<float>(
                name: "ParticipationPoints",
                table: "StudentCourses",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "GradePredictions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentId = table.Column<int>(type: "int", nullable: false),
                    CourseId = table.Column<int>(type: "int", nullable: false),
                    PredictedGrade = table.Column<float>(type: "real", nullable: false),
                    PredictionDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradePredictions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GradePredictions_Courses_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Courses",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GradePredictions_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GradePredictions_CourseId",
                table: "GradePredictions",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_GradePredictions_StudentId",
                table: "GradePredictions",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GradePredictions");

            migrationBuilder.DropColumn(
                name: "ParticipationPoints",
                table: "StudentCourses");

            migrationBuilder.AddColumn<float>(
                name: "ParticipationPoints",
                table: "Students",
                type: "real",
                nullable: false,
                defaultValue: 0f);
        }
    }
}
