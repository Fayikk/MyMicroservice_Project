using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameService.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedGameImageAndRelationShip : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Games");

            migrationBuilder.CreateTable(
                name: "GameImages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImageUrl = table.Column<string>(type: "text", nullable: true),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameImages_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameImages_GameId",
                table: "GameImages",
                column: "GameId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameImages");

            migrationBuilder.AddColumn<List<string>>(
                name: "ImageUrl",
                table: "Games",
                type: "text[]",
                nullable: true);
        }
    }
}
