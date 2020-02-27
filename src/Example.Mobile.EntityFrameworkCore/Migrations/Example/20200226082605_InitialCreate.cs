using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Example.Mobile.EntityFrameworkCore.Migrations.ExampleMobile
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "log");

            migrationBuilder.CreateTable(
                name: "Command",
                schema: "log",
                columns: table => new
                {
                    SequenceNo = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id = table.Column<Guid>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    CorrelationId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Command", x => x.SequenceNo);
                });

            migrationBuilder.CreateTable(
                name: "Event",
                schema: "log",
                columns: table => new
                {
                    SequenceNo = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id = table.Column<Guid>(nullable: false),
                    Subject = table.Column<string>(nullable: true),
                    CorrelationId = table.Column<Guid>(nullable: true),
                    CausationId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Event", x => x.SequenceNo);
                });

            migrationBuilder.CreateTable(
                name: "Query",
                schema: "log",
                columns: table => new
                {
                    SequenceNo = table.Column<long>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Id = table.Column<Guid>(nullable: false),
                    CorrelationId = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: true),
                    Data = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Query", x => x.SequenceNo);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Command_CorrelationId",
                schema: "log",
                table: "Command",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Command_Name",
                schema: "log",
                table: "Command",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Command_Subject",
                schema: "log",
                table: "Command",
                column: "Subject");

            migrationBuilder.CreateIndex(
                name: "IX_Event_CausationId",
                schema: "log",
                table: "Event",
                column: "CausationId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_CorrelationId",
                schema: "log",
                table: "Event",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Name",
                schema: "log",
                table: "Event",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_Event_Subject",
                schema: "log",
                table: "Event",
                column: "Subject");

            migrationBuilder.CreateIndex(
                name: "IX_Query_CorrelationId",
                schema: "log",
                table: "Query",
                column: "CorrelationId");

            migrationBuilder.CreateIndex(
                name: "IX_Query_Name",
                schema: "log",
                table: "Query",
                column: "Name");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Command",
                schema: "log");

            migrationBuilder.DropTable(
                name: "Event",
                schema: "log");

            migrationBuilder.DropTable(
                name: "Query",
                schema: "log");
        }
    }
}
