using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace infra.Data.Migrations
{
    /// <inheritdoc />
    public partial class _12388 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Task_ProjectTaskId",
                table: "TaskComments");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectTaskId",
                table: "TaskComments",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                table: "TaskComments",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId1",
                table: "TaskComments",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Task_ProjectTaskId",
                table: "TaskComments");

            migrationBuilder.DropForeignKey(
                name: "FK_TaskComments_Users_UserId1",
                table: "TaskComments");

            migrationBuilder.DropIndex(
                name: "IX_TaskComments_UserId1",
                table: "TaskComments");

            migrationBuilder.DropColumn(
                name: "Created",
                table: "TaskComments");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "TaskComments");

            migrationBuilder.AlterColumn<int>(
                name: "ProjectTaskId",
                table: "TaskComments",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddForeignKey(
                name: "FK_TaskComments_Task_ProjectTaskId",
                table: "TaskComments",
                column: "ProjectTaskId",
                principalTable: "Task",
                principalColumn: "Id");
        }
    }
}
