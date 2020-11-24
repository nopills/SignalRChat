using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace SignalRv2.Migrations
{
    public partial class Init02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dialogs_ChatClients_ChatClientId",
                table: "Dialogs");

            migrationBuilder.DropTable(
                name: "ChatClients");

            migrationBuilder.DropIndex(
                name: "IX_Dialogs_ChatClientId",
                table: "Dialogs");

            migrationBuilder.DropColumn(
                name: "ChatClientId",
                table: "Dialogs");

            migrationBuilder.AddColumn<bool>(
                name: "IsAdmin",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAdmin",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "ChatClientId",
                table: "Dialogs",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ChatClients",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AvatarUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastActivity = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatClients", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ChatClients_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Dialogs_ChatClientId",
                table: "Dialogs",
                column: "ChatClientId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatClients_UserId",
                table: "ChatClients",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dialogs_ChatClients_ChatClientId",
                table: "Dialogs",
                column: "ChatClientId",
                principalTable: "ChatClients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
