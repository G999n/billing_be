using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace billing_be.Migrations
{
    /// <inheritdoc />
    public partial class AddPurchaseTables2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PurchaseChalans",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ClientId = table.Column<int>(type: "integer", nullable: false),
                    InvoiceNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    InvoiceDate = table.Column<DateOnly>(type: "date", nullable: false),
                    PaymentTerms = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    TotalAmount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseChalans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseChalans_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PurchaseChalanItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    PurchaseChalanId = table.Column<int>(type: "integer", nullable: false),
                    MedicineId = table.Column<int>(type: "integer", nullable: false),
                    BatchNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ExpiryDate = table.Column<DateOnly>(type: "date", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    FreeQuantity = table.Column<int>(type: "integer", nullable: false),
                    PurchaseRate = table.Column<decimal>(type: "numeric", nullable: false),
                    Mrp = table.Column<decimal>(type: "numeric", nullable: false),
                    SalePrice = table.Column<decimal>(type: "numeric", nullable: false),
                    GstPercentage = table.Column<decimal>(type: "numeric", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseChalanItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PurchaseChalanItems_Medicines_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Medicines",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PurchaseChalanItems_PurchaseChalans_PurchaseChalanId",
                        column: x => x.PurchaseChalanId,
                        principalTable: "PurchaseChalans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseChalanItems_MedicineId",
                table: "PurchaseChalanItems",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseChalanItems_PurchaseChalanId",
                table: "PurchaseChalanItems",
                column: "PurchaseChalanId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseChalans_ClientId",
                table: "PurchaseChalans",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PurchaseChalanItems");

            migrationBuilder.DropTable(
                name: "PurchaseChalans");
        }
    }
}
