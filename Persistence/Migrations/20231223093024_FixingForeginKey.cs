using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FixingForeginKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ChannelList_ChannelListChannelId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GatewayInfo_ChannelList_ChannelListChannelId",
                table: "GatewayInfo");

            migrationBuilder.DropIndex(
                name: "IX_GatewayInfo_ChannelListChannelId",
                table: "GatewayInfo");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChannelListChannelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChannelListChannelId",
                table: "GatewayInfo");

            migrationBuilder.DropColumn(
                name: "ChannelListChannelId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ChannelId",
                table: "GatewayInfo",
                newName: "ChannelListId");

            migrationBuilder.RenameColumn(
                name: "ChannelId",
                table: "ChannelList",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ChannelID",
                table: "AspNetUsers",
                newName: "ChannelListID");

            migrationBuilder.CreateIndex(
                name: "IX_GatewayInfo_ChannelListId",
                table: "GatewayInfo",
                column: "ChannelListId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChannelListID",
                table: "AspNetUsers",
                column: "ChannelListID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ChannelList_ChannelListID",
                table: "AspNetUsers",
                column: "ChannelListID",
                principalTable: "ChannelList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GatewayInfo_ChannelList_ChannelListId",
                table: "GatewayInfo",
                column: "ChannelListId",
                principalTable: "ChannelList",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ChannelList_ChannelListID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GatewayInfo_ChannelList_ChannelListId",
                table: "GatewayInfo");

            migrationBuilder.DropIndex(
                name: "IX_GatewayInfo_ChannelListId",
                table: "GatewayInfo");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChannelListID",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ChannelListId",
                table: "GatewayInfo",
                newName: "ChannelId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ChannelList",
                newName: "ChannelId");

            migrationBuilder.RenameColumn(
                name: "ChannelListID",
                table: "AspNetUsers",
                newName: "ChannelID");

            migrationBuilder.AddColumn<int>(
                name: "ChannelListChannelId",
                table: "GatewayInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ChannelListChannelId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GatewayInfo_ChannelListChannelId",
                table: "GatewayInfo",
                column: "ChannelListChannelId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_GatewayInfo_ChannelList_ChannelListChannelId",
                table: "GatewayInfo",
                column: "ChannelListChannelId",
                principalTable: "ChannelList",
                principalColumn: "ChannelId");
        }
    }
}
