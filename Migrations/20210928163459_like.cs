using Microsoft.EntityFrameworkCore.Migrations;

namespace MyWallWebAPI.Migrations
{
    public partial class like : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "likeCount",
                table: "Post",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "likeCount",
                table: "Post");
        }
    }
}
