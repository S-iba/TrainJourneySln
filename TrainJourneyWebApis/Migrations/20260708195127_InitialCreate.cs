using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TrainJourneyWebApis.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "JourneyStages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    SequenceOrder = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_JourneyStages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GeneratedStories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    JourneyStageId = table.Column<int>(type: "int", nullable: false),
                    StoryText = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StoryTitle = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GeneratedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ImageUrl = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GeneratedStories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GeneratedStories_JourneyStages_JourneyStageId",
                        column: x => x.JourneyStageId,
                        principalTable: "JourneyStages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "JourneyStages",
                columns: new[] { "Id", "Latitude", "Longitude", "Name", "SequenceOrder" },
                values: new object[,]
                {
                    { 1, -25.747900000000001, 28.229299999999999, "Pretoria", 1 },
                    { 2, -28.728200000000001, 24.7623, "Kimberley", 2 },
                    { 3, -32.344499999999996, 22.582999999999998, "Beaufort West", 3 },
                    { 4, -33.646500000000003, 19.445900000000002, "Worcester", 4 },
                    { 5, -33.924900000000001, 18.424099999999999, "Cape Town", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GeneratedStories_JourneyStageId",
                table: "GeneratedStories",
                column: "JourneyStageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GeneratedStories");

            migrationBuilder.DropTable(
                name: "JourneyStages");
        }
    }
}
