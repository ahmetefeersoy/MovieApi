using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CommentLikeValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a9f9d5f-1d74-4c4b-b51f-a3504eb874fa");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9fea9f91-26e6-4ca6-894e-e681c933ef45");

            migrationBuilder.AddColumn<int>(
                name: "NumberOfLikes",
                table: "Comments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "0d6158ec-3319-41c7-8392-3f30a0e90026", null, "User", "USER" },
                    { "bc8e20fa-154d-4994-9c32-e3765161912d", null, "Admin", "ADMIN" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "0d6158ec-3319-41c7-8392-3f30a0e90026");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "bc8e20fa-154d-4994-9c32-e3765161912d");

            migrationBuilder.DropColumn(
                name: "NumberOfLikes",
                table: "Comments");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "3a9f9d5f-1d74-4c4b-b51f-a3504eb874fa", null, "Admin", "ADMIN" },
                    { "9fea9f91-26e6-4ca6-894e-e681c933ef45", null, "User", "USER" }
                });
        }
    }
}
