using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MyBakeryFinal.Migrations
{
    public partial class mig12 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RecipesToOrders",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Recipe_ID = table.Column<int>(type: "int", nullable: false),
                    Order_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecipesToOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RecipesToOrders_Orders_Order_ID",
                        column: x => x.Order_ID,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RecipesToOrders_Recipes_Recipe_ID",
                        column: x => x.Recipe_ID,
                        principalTable: "Recipes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RecipesToOrders_Order_ID",
                table: "RecipesToOrders",
                column: "Order_ID");

            migrationBuilder.CreateIndex(
                name: "IX_RecipesToOrders_Recipe_ID",
                table: "RecipesToOrders",
                column: "Recipe_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RecipesToOrders");
        }
    }
}
