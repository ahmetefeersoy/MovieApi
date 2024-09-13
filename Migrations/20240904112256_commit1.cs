using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class commit1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "19ba0a41-4489-44e8-8dc7-dbfb4b1d430d");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4c9c8fc7-116e-4707-a5bf-32ec920d413f");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13e80ad0-af1c-40ea-b7d5-a2e05a5066cc", null, "Admin", "ADMIN" },
                    { "82cddde8-a4f6-4452-ac1f-c03cf9147782", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13e80ad0-af1c-40ea-b7d5-a2e05a5066cc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82cddde8-a4f6-4452-ac1f-c03cf9147782");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "19ba0a41-4489-44e8-8dc7-dbfb4b1d430d", null, "Admin", "ADMIN" },
                    { "4c9c8fc7-116e-4707-a5bf-32ec920d413f", null, "User", "USER" }
                });
        }
    }
}
