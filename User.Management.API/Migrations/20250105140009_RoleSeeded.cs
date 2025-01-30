using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace User.Management.API.Migrations
{
    public partial class RoleSeeded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "0bb32a7e-25f2-45ec-9bd8-647e8087554d", "2", "User", "User" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "38920444-f745-4518-8f56-6ee48c9336fc", "3", "HR", "HR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e6382964-f1ce-4703-89ff-03067e390010", "1", "Admin", "Admin" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0bb32a7e-25f2-45ec-9bd8-647e8087554d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "38920444-f745-4518-8f56-6ee48c9336fc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e6382964-f1ce-4703-89ff-03067e390010");
        }
    }
}
