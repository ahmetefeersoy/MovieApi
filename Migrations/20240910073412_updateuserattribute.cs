using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class updateuserattribute : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "13e80ad0-af1c-40ea-b7d5-a2e05a5066cc");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "82cddde8-a4f6-4452-ac1f-c03cf9147782");

       

            migrationBuilder.AlterColumn<double>(
                name: "IMDbRating",
                table: "Films",
                type: "float",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,1)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15343c96-1201-4a7f-89a6-17f32df1d597", null, "Admin", "ADMIN" },
                    { "764acbc4-d91c-4f36-a801-814dacd0351c", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15343c96-1201-4a7f-89a6-17f32df1d597");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "764acbc4-d91c-4f36-a801-814dacd0351c");

            migrationBuilder.AlterColumn<decimal>(
                name: "IMDbRating",
                table: "Films",
                type: "decimal(18,1)",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "13e80ad0-af1c-40ea-b7d5-a2e05a5066cc", null, "Admin", "ADMIN" },
                    { "82cddde8-a4f6-4452-ac1f-c03cf9147782", null, "User", "USER" }
                });
        }
    }
}
