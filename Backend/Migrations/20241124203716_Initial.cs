using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;

#nullable disable

namespace WhoKnows_backend.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            // Step 1: Remove the existing primary key from the 'title' column
            migrationBuilder.DropPrimaryKey(
                name: "PRIMARY",
                table: "pages");

            // Step 2: Alter the 'id' column to set it as the primary key and auto-increment
            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "pages",
                type: "int",
                nullable: false,
                defaultValueSql: "AUTO_INCREMENT", // Set the column to auto increment
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PRIMARY",
                table: "pages",
                column: "id");

            // Step 3: Revert the 'title' column to just be a regular column
            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "pages",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true);

            // Additional changes to the columns if needed
        }


        /// <inheritdoc />
        //protected override void Down(MigrationBuilder migrationBuilder)
        //{
        //    migrationBuilder.DropTable(
        //        name: "pages");

        //    migrationBuilder.DropTable(
        //        name: "users");
        //}
    }
}