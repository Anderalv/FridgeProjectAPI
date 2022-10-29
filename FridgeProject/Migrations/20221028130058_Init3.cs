using Microsoft.EntityFrameworkCore.Migrations;

namespace FridgeProject.Migrations
{
    public partial class Init3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FridgeId",
                table: "FridgeProducts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProductId",
                table: "FridgeProducts",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FridgeProduct",
                columns: table => new
                {
                    FridgesId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FridgeProduct", x => new { x.FridgesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_FridgeProduct_Fridges_FridgesId",
                        column: x => x.FridgesId,
                        principalTable: "Fridges",
                        principalColumn: "IdFridge",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FridgeProduct_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "IdProduct",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Fridges_IdModel",
                table: "Fridges",
                column: "IdModel");

            migrationBuilder.CreateIndex(
                name: "IX_FridgeProducts_FridgeId",
                table: "FridgeProducts",
                column: "FridgeId");

            migrationBuilder.CreateIndex(
                name: "IX_FridgeProducts_ProductId",
                table: "FridgeProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FridgeProduct_ProductsId",
                table: "FridgeProduct",
                column: "ProductsId");

            migrationBuilder.AddForeignKey(
                name: "FK_FridgeProducts_Fridges_FridgeId",
                table: "FridgeProducts",
                column: "FridgeId",
                principalTable: "Fridges",
                principalColumn: "IdFridge",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_FridgeProducts_Products_ProductId",
                table: "FridgeProducts",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "IdProduct",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Fridges_Models_IdModel",
                table: "Fridges",
                column: "IdModel",
                principalTable: "Models",
                principalColumn: "IdModel",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FridgeProducts_Fridges_FridgeId",
                table: "FridgeProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_FridgeProducts_Products_ProductId",
                table: "FridgeProducts");

            migrationBuilder.DropForeignKey(
                name: "FK_Fridges_Models_IdModel",
                table: "Fridges");

            migrationBuilder.DropTable(
                name: "FridgeProduct");

            migrationBuilder.DropIndex(
                name: "IX_Fridges_IdModel",
                table: "Fridges");

            migrationBuilder.DropIndex(
                name: "IX_FridgeProducts_FridgeId",
                table: "FridgeProducts");

            migrationBuilder.DropIndex(
                name: "IX_FridgeProducts_ProductId",
                table: "FridgeProducts");

            migrationBuilder.DropColumn(
                name: "FridgeId",
                table: "FridgeProducts");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "FridgeProducts");
        }
    }
}
