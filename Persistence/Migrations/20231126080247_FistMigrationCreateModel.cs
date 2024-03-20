using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FistMigrationCreateModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ChannelList",
                columns: table => new
                {
                    ChannelId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ChannelName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChannelList", x => x.ChannelId);
                });

            migrationBuilder.CreateTable(
                name: "RoutingServerList",
                columns: table => new
                {
                    RID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Ip = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoutingServerList", x => x.RID);
                });

            migrationBuilder.CreateTable(
                name: "stan",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CounterDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CounterValue = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_stan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GatewayInfo",
                columns: table => new
                {
                    StanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServerReqDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GetDate()"),
                    GatewayId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Command = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LoginID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConversationID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ChannelReqDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SysChannelReqDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SysSpReqDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SpResDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SysSpResDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    SysChannelResDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ResCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResponseDesc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InitiatorID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    InitiatorKYC = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartnerID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayMode = table.Column<int>(type: "int", nullable: true),
                    PayType = table.Column<int>(type: "int", nullable: true),
                    TrxRef = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BranchCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequesterMSISDN = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InquiryMode = table.Column<int>(type: "int", nullable: true),
                    InquiryType = table.Column<int>(type: "int", nullable: true),
                    AccountID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentContractNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActualAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PayAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PartialAmountFlag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EndMonth = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FinAppResCode = table.Column<int>(type: "int", nullable: false),
                    SystemID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Event = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChannelId = table.Column<int>(type: "int", nullable: false),
                    ChannelListChannelId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatewayInfo", x => new { x.StanId, x.ServerReqDate });
                    table.ForeignKey(
                        name: "FK_GatewayInfo_ChannelList_ChannelListChannelId",
                        column: x => x.ChannelListChannelId,
                        principalTable: "ChannelList",
                        principalColumn: "ChannelId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GatewayInfo_ChannelListChannelId",
                table: "GatewayInfo",
                column: "ChannelListChannelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GatewayInfo");

            migrationBuilder.DropTable(
                name: "RoutingServerList");

            migrationBuilder.DropTable(
                name: "stan");

            migrationBuilder.DropTable(
                name: "ChannelList");
        }
    }
}
