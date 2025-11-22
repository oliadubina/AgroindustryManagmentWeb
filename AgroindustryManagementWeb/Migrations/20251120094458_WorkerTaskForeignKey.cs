using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AgroindustryManagementWeb.Migrations
{
    /// <inheritdoc />
    public partial class WorkerTaskForeignKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTasks_Fields_FieldId",
                table: "WorkerTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTasks_Workers_WorkerId",
                table: "WorkerTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTasks_Fields_FieldId",
                table: "WorkerTasks",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTasks_Workers_WorkerId",
                table: "WorkerTasks",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTasks_Fields_FieldId",
                table: "WorkerTasks");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkerTasks_Workers_WorkerId",
                table: "WorkerTasks");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTasks_Fields_FieldId",
                table: "WorkerTasks",
                column: "FieldId",
                principalTable: "Fields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkerTasks_Workers_WorkerId",
                table: "WorkerTasks",
                column: "WorkerId",
                principalTable: "Workers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
