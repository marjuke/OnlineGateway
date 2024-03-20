using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class removeForeignkeyfromuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ChannelList_ChannelListChannelId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChannelListChannelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChannelListChannelId",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ChannelListChannelId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChannelListChannelId",
                table: "AspNetUsers",
                column: "ChannelListChannelId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ChannelList_ChannelListChannelId",
                table: "AspNetUsers",
                column: "ChannelListChannelId",
                principalTable: "ChannelList",
                principalColumn: "ChannelId");
        }
    }
}
