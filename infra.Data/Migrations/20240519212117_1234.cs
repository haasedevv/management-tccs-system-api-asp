using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class _1234 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Task",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldMaxLength: 255);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Description",
                table: "Task",
                type: "integer",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }
    }
}
