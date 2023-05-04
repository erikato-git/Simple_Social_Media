using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class no_weatherforecast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WeatherForecasts");

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa311"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 20, 11, 47, 278, DateTimeKind.Utc).AddTicks(8017));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa312"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 20, 11, 47, 278, DateTimeKind.Utc).AddTicks(8047));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa313"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 20, 11, 47, 278, DateTimeKind.Utc).AddTicks(8051));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa211"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 20, 11, 47, 278, DateTimeKind.Utc).AddTicks(7956));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa212"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 20, 11, 47, 278, DateTimeKind.Utc).AddTicks(7964));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa213"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 20, 11, 47, 278, DateTimeKind.Utc).AddTicks(7968));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa111"),
                column: "DateOfBirth",
                value: new DateTime(2023, 5, 4, 20, 11, 47, 278, DateTimeKind.Utc).AddTicks(7672));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa112"),
                column: "DateOfBirth",
                value: new DateTime(2023, 5, 4, 20, 11, 47, 278, DateTimeKind.Utc).AddTicks(7697));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa113"),
                column: "DateOfBirth",
                value: new DateTime(2023, 5, 4, 20, 11, 47, 278, DateTimeKind.Utc).AddTicks(7700));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true),
                    TemperatureC = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa311"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(1053));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa312"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(1064));

            migrationBuilder.UpdateData(
                table: "Comments",
                keyColumn: "CommentId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa313"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(1073));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa211"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(983));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa212"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(992));

            migrationBuilder.UpdateData(
                table: "Posts",
                keyColumn: "PostId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa213"),
                column: "CreatedAt",
                value: new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(998));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa111"),
                column: "DateOfBirth",
                value: new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(883));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa112"),
                column: "DateOfBirth",
                value: new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(909));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "UserId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa113"),
                column: "DateOfBirth",
                value: new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(915));

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test-object", 25 });
        }
    }
}
