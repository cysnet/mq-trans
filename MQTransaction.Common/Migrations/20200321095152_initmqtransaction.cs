using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MQTransaction.Common.Migrations
{
    public partial class initmqtransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "mq_published",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false, comment: "主键"),
                    create_time = table.Column<DateTime>(nullable: false, comment: "创建时间"),
                    exchange_name = table.Column<string>(maxLength: 255, nullable: true),
                    route_key = table.Column<string>(maxLength: 255, nullable: true),
                    queue_name = table.Column<string>(maxLength: 255, nullable: false),
                    message = table.Column<string>(type: "longtext", nullable: false, comment: "消息体"),
                    status = table.Column<int>(maxLength: 11, nullable: false, comment: "发送状态"),
                    retries = table.Column<int>(maxLength: 11, nullable: false, defaultValue: 0, comment: "重试次数"),
                    expires_time = table.Column<DateTime>(nullable: true, comment: "过期时间"),
                    query_url = table.Column<string>(maxLength: 255, nullable: false, comment: "查询结果地址"),
                    remark = table.Column<string>(type: "longtext", nullable: true, comment: "备注"),
                    service_name = table.Column<string>(maxLength: 255, nullable: false, comment: "服务名")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mq_published", x => x.id);
                },
                comment: "消息发送表");

            migrationBuilder.CreateTable(
                name: "mq_received",
                columns: table => new
                {
                    id = table.Column<string>(maxLength: 36, nullable: false, comment: "主键"),
                    create_time = table.Column<DateTime>(nullable: false, comment: "创建时间"),
                    source_msg_id = table.Column<string>(maxLength: 36, nullable: false, comment: "消息发送方id-唯一"),
                    queue_name = table.Column<string>(maxLength: 255, nullable: false),
                    message = table.Column<string>(type: "longtext", nullable: false, comment: "消息体"),
                    status = table.Column<int>(maxLength: 11, nullable: false, comment: "接收状态"),
                    callback_url = table.Column<string>(maxLength: 255, nullable: false, comment: "回调地址")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_mq_received", x => x.id);
                },
                comment: "消息接收表");

            migrationBuilder.CreateIndex(
                name: "IX_mq_received_source_msg_id",
                table: "mq_received",
                column: "source_msg_id",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "mq_published");

            migrationBuilder.DropTable(
                name: "mq_received");
        }
    }
}
