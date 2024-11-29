using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhoKnows_backend.Migrations
{
    /// <inheritdoc />
    public partial class ChangePrimaryKeyAndAddIdColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the primary key on Title
            migrationBuilder.DropPrimaryKey(
                name: "PRIMARY",
                table: "pages");

            // Drop the unique index on Title if present
            migrationBuilder.DropIndex(
                name: "title",
                table: "pages");

            // Add the new Id column (assuming it's not already present)
            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "pages",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);  // Auto-increment in MySQL

            // Add the primary key constraint on Id
            migrationBuilder.AddPrimaryKey(
                name: "PRIMARY",
                table: "pages",
                column: "id");

            // Optionally, add the unique constraint for Title if you still want to keep it unique
            // migrationBuilder.CreateIndex(
            //     name: "title",
            //     table: "pages",
            //     column: "title",
            //     unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Revert the changes if the migration is rolled back
            migrationBuilder.DropPrimaryKey(
                name: "PRIMARY",
                table: "pages");

            migrationBuilder.DropColumn(
                name: "id",
                table: "pages");

            // You can re-create the primary key on Title if necessary
            migrationBuilder.AddPrimaryKey(
                name: "PRIMARY",
                table: "pages",
                column: "title");
        }
    }
}
