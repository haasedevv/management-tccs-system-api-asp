using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class hahahaha : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "TaskAttachments",
                type: "text",
                maxLength: 2147483647,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Attachment",
                table: "TaskAttachments",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldMaxLength: 2147483647);
        }
    }
}
