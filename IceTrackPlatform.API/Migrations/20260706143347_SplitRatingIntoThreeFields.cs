using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IceTrackPlatform.API.Migrations
{
    /// <inheritdoc />
    public partial class SplitRatingIntoThreeFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "rating",
                table: "reviews",
                newName: "profesionalidad");

            migrationBuilder.AddColumn<int>(
                name: "comunicacion",
                table: "reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "eficiencia",
                table: "reviews",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "comunicacion",
                table: "reviews");

            migrationBuilder.DropColumn(
                name: "eficiencia",
                table: "reviews");

            migrationBuilder.RenameColumn(
                name: "profesionalidad",
                table: "reviews",
                newName: "rating");
        }
    }
}
