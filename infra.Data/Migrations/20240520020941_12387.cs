using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class _12387 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Task_ProjectTaskId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Users_UserId1",
                table: "TaskComments");

            migrationBuilder.DropIndex(
                name: "IX_TaskComments_ProjectTaskId",
                table: "TaskComments");

            migrationBuilder.DropIndex(
                name: "IX_TaskComments_UserId1",
                table: "TaskComments");

            migrationBuilder.DropColumn(
                name: "ProjectTaskId",
                table: "TaskComments");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TaskComments");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TaskAttachments",
                type: "character varying(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ProjectTaskId",
                table: "TaskComments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "TaskComments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TaskAttachments",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(255)",
                oldMaxLength: 255);

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_ProjectTaskId",
                table: "TaskComments",
                column: "ProjectTaskId");

            migrationBuilder.CreateIndex(
                name: "IX_TaskComments_UserId1",
                table: "TaskComments",
                column: "UserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Task_ProjectTaskId",
                table: "TaskComments",
                column: "ProjectTaskId",
                principalTable: "Task",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Users_UserId1",
                table: "TaskComments",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
