using Microsoft.EntityFrameworkCore.Migrations;

namespace FridgeProject.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "FridgeProducts",
                columns: new[] { "IdFridgeProduct", "IdFridge", "IdProduct", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 1 },
                    { 14, 4, 5, 4 },
                    { 13, 4, 4, 4 },
                    { 11, 3, 4, 5 },
                    { 10, 3, 3, 5 },
                    { 9, 2, 5, 5 },
                    { 8, 2, 4, 4 },
                    { 12, 3, 5, 5 },
                    { 6, 2, 2, 2 },
                    { 5, 1, 5, 5 },
                    { 4, 1, 4, 4 },
                    { 3, 1, 3, 3 },
                    { 2, 1, 2, 2 },
                    { 7, 2, 3, 3 }
                });

            migrationBuilder.InsertData(
                table: "Models",
                columns: new[] { "IdModel", "Name", "Year" },
                values: new object[,]
                {
                    { 1, "Atlant", 2001 },
                    { 2, "Vestfrost", 2002 },
                    { 3, "Mitsubishi", 2003 },
                    { 4, "Bosch", 2004 },
                    { 5, "Samsung", 2005 }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "IdProduct", "DefaultQuantity", "Name" },
                values: new object[,]
                {
                    { 4, 4, "Carrot" },
                    { 1, 1, "Milk" },
                    { 2, 2, "Bacon" },
                    { 3, 3, "Beans" },
                    { 5, 5, "Apple" }
                });

            migrationBuilder.InsertData(
                table: "Fridges",
                columns: new[] { "IdFridge", "IdModel", "Name", "OwnerName" },
                values: new object[,]
                {
                    { 1, 1, "Fridge1", "Ivan" },
                    { 2, 1, "Fridge2", "Andrey" },
                    { 3, 2, "Fridge3", "Dima" },
                    { 4, 3, "Fridge4", "Vova" },
                    { 5, 3, "Fridge5", "Egor" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "FridgeProducts",
                keyColumn: "IdFridgeProduct",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Fridges",
                keyColumn: "IdFridge",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Fridges",
                keyColumn: "IdFridge",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Fridges",
                keyColumn: "IdFridge",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Fridges",
                keyColumn: "IdFridge",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Fridges",
                keyColumn: "IdFridge",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "IdModel",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "IdModel",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "IdProduct",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "IdProduct",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "IdProduct",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "IdProduct",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "IdProduct",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "IdModel",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "IdModel",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Models",
                keyColumn: "IdModel",
                keyValue: 3);
        }
    }
}
