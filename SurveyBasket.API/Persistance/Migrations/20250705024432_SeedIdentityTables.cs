using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SurveyBasket.API.Persistance.Migrations
{
    /// <inheritdoc />
    public partial class SeedIdentityTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "IsDefault", "IsDeleted", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "035611FF-9EA0-4620-816F-05ECCE40EB6F", "F84B860E-E63B-4BBF-B9F0-EBF2CB9CCC06", false, false, "Admin", "ADMIN" },
                    { "942E0E3B-A748-4205-9300-1A7950957FE4", "A78D645B-9E0B-4C55-AD9B-50AB4F682062", true, false, "Member", "MEMBER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "Email", "EmailConfirmed", "FirstName", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "0197d797-a577-749e-a9c9-31b55c5a803f", 0, "F57EA779-0B85-45B0-9694-7C4DAF1E0E06", "admin@survey-busket.com", true, "Survey Busket", "Admin", false, null, "ADMIN@SURVEY-BUSKET.COM", "ADMIN@SURVEY-BUSKET.COM", "AQAAAAIAAYagAAAAEHjXPM/W5gcdJHv3yTU9N1jFfHSwZD/UUl0NFvyb0YI8k5XxK+bvgZs6oaSztFvwjQ==", null, false, "1666E52998734B97853FE4CF8B624966", false, "admin@survey-busket.com" });

            migrationBuilder.InsertData(
                table: "AspNetRoleClaims",
                columns: new[] { "Id", "ClaimType", "ClaimValue", "RoleId" },
                values: new object[,]
                {
                    { 1, "Permissions", "polls:read", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 2, "Permissions", "polls:Add", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 3, "Permissions", "polls:Update", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 4, "Permissions", "polls:Delete", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 5, "Permissions", "questions:read", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 6, "Permissions", "questions:Add", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 7, "Permissions", "questions:Update", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 8, "Permissions", "users:read", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 9, "Permissions", "users:add", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 10, "Permissions", "users:update", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 11, "Permissions", "roles:read", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 12, "Permissions", "roles:add", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 13, "Permissions", "roles:update", "035611FF-9EA0-4620-816F-05ECCE40EB6F" },
                    { 14, "Permissions", "result:read", "035611FF-9EA0-4620-816F-05ECCE40EB6F" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { "035611FF-9EA0-4620-816F-05ECCE40EB6F", "0197d797-a577-749e-a9c9-31b55c5a803f" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "AspNetRoleClaims",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "942E0E3B-A748-4205-9300-1A7950957FE4");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "035611FF-9EA0-4620-816F-05ECCE40EB6F", "0197d797-a577-749e-a9c9-31b55c5a803f" });

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "035611FF-9EA0-4620-816F-05ECCE40EB6F");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "0197d797-a577-749e-a9c9-31b55c5a803f");
        }
    }
}
