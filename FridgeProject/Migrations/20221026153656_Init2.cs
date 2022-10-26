using Microsoft.EntityFrameworkCore.Migrations;

namespace FridgeProject.Migrations
{
    public partial class Init2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fridges_Models_IdModel",
                table: "Fridges");

            migrationBuilder.DropIndex(
                name: "IX_Fridges_IdModel",
                table: "Fridges");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Fridges_IdModel",
                table: "Fridges",
                column: "IdModel");

            migrationBuilder.AddForeignKey(
                name: "FK_Fridges_Models_IdModel",
                table: "Fridges",
                column: "IdModel",
                principalTable: "Models",
                principalColumn: "IdModel",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
