using Microsoft.EntityFrameworkCore.Migrations;

namespace BulletinBoard.Migrations
{
    public partial class Many2manySetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Post_Category_CategoryID",
                table: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Post_CategoryID",
                table: "Post");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Post");

            migrationBuilder.CreateTable(
                name: "PostCategory",
                columns: table => new
                {
                    PostID = table.Column<int>(nullable: false),
                    CategoryID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostCategory", x => new { x.PostID, x.CategoryID });
                    table.ForeignKey(
                        name: "FK_PostCategory_Category_CategoryID",
                        column: x => x.CategoryID,
                        principalTable: "Category",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PostCategory_Post_PostID",
                        column: x => x.PostID,
                        principalTable: "Post",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PostCategory_CategoryID",
                table: "PostCategory",
                column: "CategoryID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PostCategory");

            migrationBuilder.AddColumn<int>(
                name: "CategoryID",
                table: "Post",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Post_CategoryID",
                table: "Post",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Post_Category_CategoryID",
                table: "Post",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
