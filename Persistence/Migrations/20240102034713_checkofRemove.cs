using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class checkofRemove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentConfirmReqDate",
                table: "GatewayInfo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentConfirmResCode",
                table: "GatewayInfo",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentConfirmResDate",
                table: "GatewayInfo",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentConfirmResDesc",
                table: "GatewayInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentConfirmResNote",
                table: "GatewayInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SystemTrID",
                table: "GatewayInfo",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "GatewayCheckInfo",
                columns: table => new
                {
                    StanId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServerReqDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GetDate()"),
                    GatewayId = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatewayCheckInfo", x => new { x.StanId, x.ServerReqDate });
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GatewayCheckInfo");

            migrationBuilder.DropColumn(
                name: "PaymentConfirmReqDate",
                table: "GatewayInfo");

            migrationBuilder.DropColumn(
                name: "PaymentConfirmResCode",
                table: "GatewayInfo");

            migrationBuilder.DropColumn(
                name: "PaymentConfirmResDate",
                table: "GatewayInfo");

            migrationBuilder.DropColumn(
                name: "PaymentConfirmResDesc",
                table: "GatewayInfo");

            migrationBuilder.DropColumn(
                name: "PaymentConfirmResNote",
                table: "GatewayInfo");

            migrationBuilder.DropColumn(
                name: "SystemTrID",
                table: "GatewayInfo");
        }
    }
}
