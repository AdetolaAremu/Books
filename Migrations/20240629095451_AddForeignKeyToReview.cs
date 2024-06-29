using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookReview.Migrations
{
    /// <inheritdoc />
    public partial class AddForeignKeyToReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Books_BooksId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Reviewers_ReviewersId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "ReviewersId",
                table: "Reviews",
                newName: "ReviewerId");

            migrationBuilder.RenameColumn(
                name: "BooksId",
                table: "Reviews",
                newName: "BookId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ReviewersId",
                table: "Reviews",
                newName: "IX_Reviews_ReviewerId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_BooksId",
                table: "Reviews",
                newName: "IX_Reviews_BookId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Books_BookId",
                table: "Reviews",
                column: "BookId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Reviewers_ReviewerId",
                table: "Reviews",
                column: "ReviewerId",
                principalTable: "Reviewers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Books_BookId",
                table: "Reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Reviewers_ReviewerId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "ReviewerId",
                table: "Reviews",
                newName: "ReviewersId");

            migrationBuilder.RenameColumn(
                name: "BookId",
                table: "Reviews",
                newName: "BooksId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_ReviewerId",
                table: "Reviews",
                newName: "IX_Reviews_ReviewersId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_BookId",
                table: "Reviews",
                newName: "IX_Reviews_BooksId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Books_BooksId",
                table: "Reviews",
                column: "BooksId",
                principalTable: "Books",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Reviewers_ReviewersId",
                table: "Reviews",
                column: "ReviewersId",
                principalTable: "Reviewers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
