using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskModules.Migrations
{
    public partial class AddDepartmentlogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DepartmentLogo",
                table: "Department",
                type: "varchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DepartmentLogo",
                table: "Department");
        }
    }
}
