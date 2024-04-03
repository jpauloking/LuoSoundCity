using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApp.Migrations
{
    public partial class AddExtraFieldsToChartSongJoinTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartSong_Chart_ChartsId",
                table: "ChartSong");

            migrationBuilder.DropForeignKey(
                name: "FK_ChartSong_Song_SongsId",
                table: "ChartSong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChartSong",
                table: "ChartSong");

            migrationBuilder.DropIndex(
                name: "IX_ChartSong_SongsId",
                table: "ChartSong");

            migrationBuilder.RenameColumn(
                name: "SongsId",
                table: "ChartSong",
                newName: "PreviousPosition");

            migrationBuilder.RenameColumn(
                name: "ChartsId",
                table: "ChartSong",
                newName: "Position");

            migrationBuilder.RenameColumn(
                name: "DisplayImageURL",
                table: "Chart",
                newName: "DisplayImageUrl");

            migrationBuilder.AddColumn<int>(
                name: "ChartId",
                table: "ChartSong",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SongId",
                table: "ChartSong",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateAddedToChart",
                table: "ChartSong",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSongReachedPosition",
                table: "ChartSong",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWeeksAtPosition",
                table: "ChartSong",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfWeeksOnChart",
                table: "ChartSong",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChartSong",
                table: "ChartSong",
                columns: new[] { "ChartId", "SongId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChartSong_SongId",
                table: "ChartSong",
                column: "SongId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChartSong_Chart_ChartId",
                table: "ChartSong",
                column: "ChartId",
                principalTable: "Chart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChartSong_Song_SongId",
                table: "ChartSong",
                column: "SongId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChartSong_Chart_ChartId",
                table: "ChartSong");

            migrationBuilder.DropForeignKey(
                name: "FK_ChartSong_Song_SongId",
                table: "ChartSong");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ChartSong",
                table: "ChartSong");

            migrationBuilder.DropIndex(
                name: "IX_ChartSong_SongId",
                table: "ChartSong");

            migrationBuilder.DropColumn(
                name: "ChartId",
                table: "ChartSong");

            migrationBuilder.DropColumn(
                name: "SongId",
                table: "ChartSong");

            migrationBuilder.DropColumn(
                name: "DateAddedToChart",
                table: "ChartSong");

            migrationBuilder.DropColumn(
                name: "DateSongReachedPosition",
                table: "ChartSong");

            migrationBuilder.DropColumn(
                name: "NumberOfWeeksAtPosition",
                table: "ChartSong");

            migrationBuilder.DropColumn(
                name: "NumberOfWeeksOnChart",
                table: "ChartSong");

            migrationBuilder.RenameColumn(
                name: "PreviousPosition",
                table: "ChartSong",
                newName: "SongsId");

            migrationBuilder.RenameColumn(
                name: "Position",
                table: "ChartSong",
                newName: "ChartsId");

            migrationBuilder.RenameColumn(
                name: "DisplayImageUrl",
                table: "Chart",
                newName: "DisplayImageURL");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ChartSong",
                table: "ChartSong",
                columns: new[] { "ChartsId", "SongsId" });

            migrationBuilder.CreateIndex(
                name: "IX_ChartSong_SongsId",
                table: "ChartSong",
                column: "SongsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ChartSong_Chart_ChartsId",
                table: "ChartSong",
                column: "ChartsId",
                principalTable: "Chart",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChartSong_Song_SongsId",
                table: "ChartSong",
                column: "SongsId",
                principalTable: "Song",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
