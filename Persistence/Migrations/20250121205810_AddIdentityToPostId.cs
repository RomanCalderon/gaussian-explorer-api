using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIdentityToPostId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Post",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Posts",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "Posts");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Posts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Post",
                table: "Posts",
                column: "Id");
        }
    }
}
