using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AppUserUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4319d61b-034c-496b-8782-295175b2db86");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f66bbc88-23c0-4112-be1f-f33cc905635f");

            migrationBuilder.AddColumn<string>(
                name: "ProfilImageUrl",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3a9f9d5f-1d74-4c4b-b51f-a3504eb874fa", null, "Admin", "ADMIN" },
                    { "9fea9f91-26e6-4ca6-894e-e681c933ef45", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a9f9d5f-1d74-4c4b-b51f-a3504eb874fa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fea9f91-26e6-4ca6-894e-e681c933ef45");

            migrationBuilder.DropColumn(
                name: "ProfilImageUrl",
                table: "AspNetUsers");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4319d61b-034c-496b-8782-295175b2db86", null, "User", "USER" },
                    { "f66bbc88-23c0-4112-be1f-f33cc905635f", null, "Admin", "ADMIN" }
                });
        }
    }
}
