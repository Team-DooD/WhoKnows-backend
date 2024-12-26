using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhoKnows_backend.Migrations
{
    /// <inheritdoc />
    public partial class changes3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "title",
                table: "pages");

            migrationBuilder.CreateIndex(
                name: "title",
                table: "pages",
                column: "title");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "title",
                table: "pages");

            migrationBuilder.CreateIndex(
                name: "title",
                table: "pages",
                column: "title",
                unique: true);
        }
    }
}