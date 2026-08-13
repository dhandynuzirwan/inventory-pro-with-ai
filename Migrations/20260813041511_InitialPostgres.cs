using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace InventorySystem.Migrations
{
    /// <inheritdoc />
    public partial class InitialPostgres : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    SKU = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CategoryId = table.Column<int>(type: "integer", nullable: false),
                    Price = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false),
                    MinimumStock = table.Column<int>(type: "integer", nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StockTransactions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    ProductId = table.Column<int>(type: "integer", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    TransactionDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTransactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTransactions_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Perangkat elektronik dan gadget", "Elektronik" },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Perlengkapan alat tulis kantor", "Alat Tulis" },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Perabot kantor dan rumah tangga", "Furnitur" },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Produk konsumsi makanan dan minuman", "Makanan & Minuman" },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pakaian dan aksesoris", "Pakaian" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Description", "MinimumStock", "Name", "Price", "SKU", "Stock" },
                values: new object[,]
                {
                    { 1, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Laptop 14 inch, RAM 8GB, SSD 512GB", 5, "Laptop ASUS VivoBook", 8500000m, "ELK-001", 25 },
                    { 2, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mouse wireless ergonomis", 15, "Mouse Wireless Logitech", 350000m, "ELK-002", 50 },
                    { 3, 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Keyboard mechanical RGB", 10, "Keyboard Mechanical", 750000m, "ELK-003", 3 },
                    { 4, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pulpen gel 0.5mm", 50, "Pulpen Pilot G2", 15000m, "ATK-001", 200 },
                    { 5, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Buku tulis 100 lembar", 30, "Buku Tulis A5", 8000m, "ATK-002", 150 },
                    { 6, 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kertas HVS 80gsm 500 lembar", 20, "Kertas HVS A4", 55000m, "ATK-003", 8 },
                    { 7, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Meja kerja kayu 120x60cm", 3, "Meja Kerja Minimalis", 1200000m, "FRN-001", 12 },
                    { 8, 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kursi kantor dengan sandaran mesh", 5, "Kursi Kantor Ergonomis", 2500000m, "FRN-002", 2 },
                    { 9, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Air mineral kemasan botol", 100, "Air Mineral 600ml", 4000m, "MKN-001", 500 },
                    { 10, 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kopi instan sachet 25g", 50, "Kopi Sachet Kapal Api", 2500m, "MKN-002", 5 },
                    { 11, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kaos polo cotton combed 30s", 10, "Kaos Polo Hitam", 125000m, "PKN-001", 40 },
                    { 12, 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kemeja katun formal lengan panjang", 10, "Kemeja Putih Formal", 185000m, "PKN-002", 7 }
                });

            migrationBuilder.InsertData(
                table: "StockTransactions",
                columns: new[] { "Id", "Notes", "ProductId", "Quantity", "TransactionDate", "Type" },
                values: new object[,]
                {
                    { 1, "Stok awal", 1, 30, new DateTime(2024, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 2, "Penjualan", 1, 5, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, "Restok dari supplier", 2, 60, new DateTime(2024, 1, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 4, "Penjualan online", 2, 10, new DateTime(2024, 3, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 5, "Stok awal", 3, 15, new DateTime(2024, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 6, "Penjualan grosir", 3, 12, new DateTime(2024, 3, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 7, "Pembelian bulk", 4, 250, new DateTime(2024, 2, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 8, "Distribusi ke cabang", 4, 50, new DateTime(2024, 3, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 9, "Pengadaan kantor baru", 8, 10, new DateTime(2024, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 10, "Distribusi ke kantor cabang", 8, 8, new DateTime(2024, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 11, "Stok awal", 10, 100, new DateTime(2024, 1, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), 0 },
                    { 12, "Pemakaian harian", 10, 95, new DateTime(2024, 3, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTransactions_ProductId",
                table: "StockTransactions",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockTransactions");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
