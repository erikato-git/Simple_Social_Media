﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace webapi.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    Salt = table.Column<int>(type: "integer", nullable: false),
                    Full_Name = table.Column<string>(type: "text", nullable: false),
                    Profile_Picture = table.Column<string>(type: "text", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "WeatherForecasts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TemperatureC = table.Column<int>(type: "integer", nullable: false),
                    Summary = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WeatherForecasts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: true),
                    Image = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.PostId);
                    table.ForeignKey(
                        name: "FK_Posts_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    CommentId = table.Column<Guid>(type: "uuid", nullable: false),
                    Content = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    PostId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "PostId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "UserId", "DateOfBirth", "Description", "Email", "Full_Name", "Password", "Profile_Picture", "Salt" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa111"), new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(883), null, "user1@mail.com", "user 1", "123", null, 0 },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa112"), new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(909), null, "user2@mail.com", "user 2", "123", null, 0 },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa113"), new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(915), null, "user3@mail.com", "user 3", "123", null, 0 }
                });

            migrationBuilder.InsertData(
                table: "WeatherForecasts",
                columns: new[] { "Id", "Date", "Summary", "TemperatureC" },
                values: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaa1"), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Test-object", 25 });

            migrationBuilder.InsertData(
                table: "Posts",
                columns: new[] { "PostId", "Content", "CreatedAt", "Image", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa211"), "user 1", new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(983), null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa111") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa212"), "user 2", new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(992), null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa112") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa213"), "user 3", new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(998), null, new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa113") }
                });

            migrationBuilder.InsertData(
                table: "Comments",
                columns: new[] { "CommentId", "Content", "CreatedAt", "PostId", "UserId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa311"), "comment 1", new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(1053), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa211"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa111") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa312"), "comment 2", new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(1064), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa212"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa112") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa313"), "comment 3", new DateTime(2023, 5, 4, 16, 17, 57, 625, DateTimeKind.Utc).AddTicks(1073), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa211"), new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaa111") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_UserId",
                table: "Comments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_UserId",
                table: "Posts",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "WeatherForecasts");

            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
