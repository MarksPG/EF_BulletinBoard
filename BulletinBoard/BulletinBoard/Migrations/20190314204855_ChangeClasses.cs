using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BulletinBoard.Migrations
{
    public partial class ChangeClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Text",
                table: "Post",
                newName: "Content");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Post",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Like",
                table: "Post",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Topic",
                table: "Post",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "Post",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserID",
                table: "Post",
                column: "UserID");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_User_UserID",
                table: "Post",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_User_UserID",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_UserID",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Like",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "Topic",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "UserID",
                table: "Post");

            migrationBuilder.RenameColumn(
                name: "Content",
                table: "Post",
                newName: "Text");
        }
    }
}
