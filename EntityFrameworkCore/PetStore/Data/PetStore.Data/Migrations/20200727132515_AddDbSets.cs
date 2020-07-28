using Microsoft.EntityFrameworkCore.Migrations;

namespace PetStore.Data.Migrations
{
    public partial class AddDbSets : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryFood_DistributorDelivery_DeliveryId",
                table: "DeliveryFood");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryFood_Foods_FoodId",
                table: "DeliveryFood");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryToy_DistributorDelivery_DeliveryId",
                table: "DeliveryToy");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryToy_Toys_ToyId",
                table: "DeliveryToy");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributorDelivery_Distributor_DistributorId",
                table: "DistributorDelivery");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributorDelivery_Orders_OrderId",
                table: "DistributorDelivery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DistributorDelivery",
                table: "DistributorDelivery");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Distributor",
                table: "Distributor");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryToy",
                table: "DeliveryToy");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryFood",
                table: "DeliveryFood");

            migrationBuilder.RenameTable(
                name: "DistributorDelivery",
                newName: "DistributorDeliveries");

            migrationBuilder.RenameTable(
                name: "Distributor",
                newName: "Distributors");

            migrationBuilder.RenameTable(
                name: "DeliveryToy",
                newName: "DeliveryToies");

            migrationBuilder.RenameTable(
                name: "DeliveryFood",
                newName: "DeliveryFoods");

            migrationBuilder.RenameIndex(
                name: "IX_DistributorDelivery_OrderId",
                table: "DistributorDeliveries",
                newName: "IX_DistributorDeliveries_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributorDelivery_DistributorId",
                table: "DistributorDeliveries",
                newName: "IX_DistributorDeliveries_DistributorId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryToy_ToyId",
                table: "DeliveryToies",
                newName: "IX_DeliveryToies_ToyId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryFood_FoodId",
                table: "DeliveryFoods",
                newName: "IX_DeliveryFoods_FoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DistributorDeliveries",
                table: "DistributorDeliveries",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Distributors",
                table: "Distributors",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryToies",
                table: "DeliveryToies",
                columns: new[] { "DeliveryId", "ToyId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryFoods",
                table: "DeliveryFoods",
                columns: new[] { "DeliveryId", "FoodId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryFoods_DistributorDeliveries_DeliveryId",
                table: "DeliveryFoods",
                column: "DeliveryId",
                principalTable: "DistributorDeliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryFoods_Foods_FoodId",
                table: "DeliveryFoods",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryToies_DistributorDeliveries_DeliveryId",
                table: "DeliveryToies",
                column: "DeliveryId",
                principalTable: "DistributorDeliveries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryToies_Toys_ToyId",
                table: "DeliveryToies",
                column: "ToyId",
                principalTable: "Toys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorDeliveries_Distributors_DistributorId",
                table: "DistributorDeliveries",
                column: "DistributorId",
                principalTable: "Distributors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorDeliveries_Orders_OrderId",
                table: "DistributorDeliveries",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryFoods_DistributorDeliveries_DeliveryId",
                table: "DeliveryFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryFoods_Foods_FoodId",
                table: "DeliveryFoods");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryToies_DistributorDeliveries_DeliveryId",
                table: "DeliveryToies");

            migrationBuilder.DropForeignKey(
                name: "FK_DeliveryToies_Toys_ToyId",
                table: "DeliveryToies");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributorDeliveries_Distributors_DistributorId",
                table: "DistributorDeliveries");

            migrationBuilder.DropForeignKey(
                name: "FK_DistributorDeliveries_Orders_OrderId",
                table: "DistributorDeliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Distributors",
                table: "Distributors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DistributorDeliveries",
                table: "DistributorDeliveries");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryToies",
                table: "DeliveryToies");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DeliveryFoods",
                table: "DeliveryFoods");

            migrationBuilder.RenameTable(
                name: "Distributors",
                newName: "Distributor");

            migrationBuilder.RenameTable(
                name: "DistributorDeliveries",
                newName: "DistributorDelivery");

            migrationBuilder.RenameTable(
                name: "DeliveryToies",
                newName: "DeliveryToy");

            migrationBuilder.RenameTable(
                name: "DeliveryFoods",
                newName: "DeliveryFood");

            migrationBuilder.RenameIndex(
                name: "IX_DistributorDeliveries_OrderId",
                table: "DistributorDelivery",
                newName: "IX_DistributorDelivery_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_DistributorDeliveries_DistributorId",
                table: "DistributorDelivery",
                newName: "IX_DistributorDelivery_DistributorId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryToies_ToyId",
                table: "DeliveryToy",
                newName: "IX_DeliveryToy_ToyId");

            migrationBuilder.RenameIndex(
                name: "IX_DeliveryFoods_FoodId",
                table: "DeliveryFood",
                newName: "IX_DeliveryFood_FoodId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Distributor",
                table: "Distributor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DistributorDelivery",
                table: "DistributorDelivery",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryToy",
                table: "DeliveryToy",
                columns: new[] { "DeliveryId", "ToyId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_DeliveryFood",
                table: "DeliveryFood",
                columns: new[] { "DeliveryId", "FoodId" });

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryFood_DistributorDelivery_DeliveryId",
                table: "DeliveryFood",
                column: "DeliveryId",
                principalTable: "DistributorDelivery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryFood_Foods_FoodId",
                table: "DeliveryFood",
                column: "FoodId",
                principalTable: "Foods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryToy_DistributorDelivery_DeliveryId",
                table: "DeliveryToy",
                column: "DeliveryId",
                principalTable: "DistributorDelivery",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DeliveryToy_Toys_ToyId",
                table: "DeliveryToy",
                column: "ToyId",
                principalTable: "Toys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorDelivery_Distributor_DistributorId",
                table: "DistributorDelivery",
                column: "DistributorId",
                principalTable: "Distributor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DistributorDelivery_Orders_OrderId",
                table: "DistributorDelivery",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
