using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PullPitcher.Migrations
{
    /// <inheritdoc />
    public partial class RemoveRepoColummn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Repository",
                table: "AppPitcherIndecies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Repository",
                table: "AppPitcherIndecies",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
