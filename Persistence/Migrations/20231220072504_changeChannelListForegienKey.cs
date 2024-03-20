using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changeChannelListForegienKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ChannelList_ChannelID",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GatewayInfo_ChannelList_ChannelListChannelId",
                table: "GatewayInfo");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChannelID",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ChannelListChannelId",
                table: "GatewayInfo",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

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

            migrationBuilder.AddForeignKey(
                name: "FK_GatewayInfo_ChannelList_ChannelListChannelId",
                table: "GatewayInfo",
                column: "ChannelListChannelId",
                principalTable: "ChannelList",
                principalColumn: "ChannelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_ChannelList_ChannelListChannelId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_GatewayInfo_ChannelList_ChannelListChannelId",
                table: "GatewayInfo");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_ChannelListChannelId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "ChannelListChannelId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "ChannelListChannelId",
                table: "GatewayInfo",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_ChannelID",
                table: "AspNetUsers",
                column: "ChannelID");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_ChannelList_ChannelID",
                table: "AspNetUsers",
                column: "ChannelID",
                principalTable: "ChannelList",
                principalColumn: "ChannelId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_GatewayInfo_ChannelList_ChannelListChannelId",
                table: "GatewayInfo",
                column: "ChannelListChannelId",
                principalTable: "ChannelList",
                principalColumn: "ChannelId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
