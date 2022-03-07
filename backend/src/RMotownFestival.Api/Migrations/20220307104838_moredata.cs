using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RMotownFestival.Api.Migrations
{
    public partial class moredata : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ScheduleItems",
                columns: new[] { "Id", "ArtistId", "IsFavorite", "ScheduleId", "StageId", "Time" },
                values: new object[,]
                {
                    { 1, 1, false, 1, 1, new DateTime(1972, 7, 1, 20, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 5, false, 1, 2, new DateTime(1972, 7, 1, 20, 30, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 3, false, 1, 1, new DateTime(1972, 7, 1, 22, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 2, false, 1, 2, new DateTime(1972, 7, 1, 22, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 1, false, 1, 1, new DateTime(1972, 7, 2, 20, 15, 0, 0, DateTimeKind.Unspecified) },
                    { 6, 5, false, 1, 2, new DateTime(1972, 7, 2, 20, 45, 0, 0, DateTimeKind.Unspecified) },
                    { 7, 4, false, 1, 1, new DateTime(1972, 7, 2, 22, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, 2, false, 1, 2, new DateTime(1972, 7, 2, 22, 30, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ScheduleItems",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ScheduleItems",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ScheduleItems",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ScheduleItems",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ScheduleItems",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "ScheduleItems",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "ScheduleItems",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "ScheduleItems",
                keyColumn: "Id",
                keyValue: 8);
        }
    }
}
