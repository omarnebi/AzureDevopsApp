using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AzureDevopsApp.Migrations
{
    /// <inheritdoc />
    public partial class app5 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Projects",
                table: "Projects");

            migrationBuilder.RenameTable(
                name: "Projects",
                newName: "ProjectInfos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ProjectInfos",
                table: "ProjectInfos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_ProjectInfos",
                table: "ProjectInfos");

            migrationBuilder.RenameTable(
                name: "ProjectInfos",
                newName: "Projects");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Projects",
                table: "Projects",
                column: "Id");
        }
    }
}
