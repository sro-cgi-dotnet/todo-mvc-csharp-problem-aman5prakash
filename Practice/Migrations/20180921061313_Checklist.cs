using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Practice.Migrations
{
    public partial class Checklist : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CheckList",
                columns: table => new
                {
                    ChecklistID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ChecklistText = table.Column<string>(nullable: true),
                    KeepId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CheckList", x => x.ChecklistID);
                    table.ForeignKey(
                        name: "FK_CheckList_Google_KeepId",
                        column: x => x.KeepId,
                        principalTable: "Google",
                        principalColumn: "KeepId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CheckList_KeepId",
                table: "CheckList",
                column: "KeepId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CheckList");
        }
    }
}
