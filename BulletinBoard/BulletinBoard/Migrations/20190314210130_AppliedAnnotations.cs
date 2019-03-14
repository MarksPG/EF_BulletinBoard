using Microsoft.EntityFrameworkCore.Migrations;

namespace BulletinBoard.Migrations
{
    public partial class AppliedAnnotations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Category_CategoryID",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_User_UserID",
                table: "Post");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Post",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Like",
                table: "Post",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Post",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Category_CategoryID",
                table: "Post",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_User_UserID",
                table: "Post",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Category_CategoryID",
                table: "Post");

            migrationBuilder.DropForeignKey(
                name: "FK_Post_User_UserID",
                table: "Post");

            migrationBuilder.AlterColumn<int>(
                name: "UserID",
                table: "Post",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "Like",
                table: "Post",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CategoryID",
                table: "Post",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Category_CategoryID",
                table: "Post",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Post_User_UserID",
                table: "Post",
                column: "UserID",
                principalTable: "User",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
