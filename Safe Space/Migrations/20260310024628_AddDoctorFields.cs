using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SafeSpace.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileImageUrl",
                table: "Doctors",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Rating",
                table: "Doctors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<int>(
                name: "ReviewsCount",
                table: "Doctors",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileImageUrl",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "ReviewsCount",
                table: "Doctors");
        }
    }
}
