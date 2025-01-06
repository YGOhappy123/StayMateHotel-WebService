using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace server.Data.Migrations
{
    /// <inheritdoc />
    public partial class ChangeIsAcceptedFlagToStatusEnum : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccepted",
                table: "BookingServices");

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "BookingServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "BookingServices");

            migrationBuilder.AddColumn<bool>(
                name: "IsAccepted",
                table: "BookingServices",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
