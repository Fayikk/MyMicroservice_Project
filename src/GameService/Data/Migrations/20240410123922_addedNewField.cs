using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameService.Data.Migrations
{
    /// <inheritdoc />
    public partial class addedNewField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GameInfo",
                table: "Games",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GameInfo",
                table: "Games");
        }
    }
}
