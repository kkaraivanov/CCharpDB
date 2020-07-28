using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PetStore.Data.Migrations
{
    public partial class AddNewClasses : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Toys",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                table: "Foods",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Distributor",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    BankAccount = table.Column<string>(maxLength: 27, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Distributor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DistributorDelivery",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DeliveryDate = table.Column<DateTime>(nullable: false),
                    Cost = table.Column<decimal>(nullable: false),
                    DistributorId = table.Column<int>(nullable: false),
                    OrderId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DistributorDelivery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DistributorDelivery_Distributor_DistributorId",
                        column: x => x.DistributorId,
                        principalTable: "Distributor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DistributorDelivery_Orders_OrderId",
                        column: x => x.OrderId,
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryFood",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(nullable: false),
                    FoodId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryFood", x => new { x.DeliveryId, x.FoodId });
                    table.ForeignKey(
                        name: "FK_DeliveryFood_DistributorDelivery_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "DistributorDelivery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryFood_Foods_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Foods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeliveryToy",
                columns: table => new
                {
                    DeliveryId = table.Column<int>(nullable: false),
                    ToyId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeliveryToy", x => new { x.DeliveryId, x.ToyId });
                    table.ForeignKey(
                        name: "FK_DeliveryToy_DistributorDelivery_DeliveryId",
                        column: x => x.DeliveryId,
                        principalTable: "DistributorDelivery",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeliveryToy_Toys_ToyId",
                        column: x => x.ToyId,
                        principalTable: "Toys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryFood_FoodId",
                table: "DeliveryFood",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_DeliveryToy_ToyId",
                table: "DeliveryToy",
                column: "ToyId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributorDelivery_DistributorId",
                table: "DistributorDelivery",
                column: "DistributorId");

            migrationBuilder.CreateIndex(
                name: "IX_DistributorDelivery_OrderId",
                table: "DistributorDelivery",
                column: "OrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeliveryFood");

            migrationBuilder.DropTable(
                name: "DeliveryToy");

            migrationBuilder.DropTable(
                name: "DistributorDelivery");

            migrationBuilder.DropTable(
                name: "Distributor");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Toys");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "Foods");
        }
    }
}
