using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhoKnows_backend.Migrations
{
    /// <inheritdoc />
    public partial class changes5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedBy",
                table: "pages",
                newName: "createdBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "createdBy",
                table: "pages",
                newName: "CreatedBy");
        }
    }
}