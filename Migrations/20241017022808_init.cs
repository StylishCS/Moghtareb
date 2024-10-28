using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moghtareb.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    whatsapp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    university = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    age = table.Column<int>(type: "int", nullable: true),
                    degree = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    isFeatured = table.Column<bool>(type: "bit", nullable: false),
                    isSecured = table.Column<bool>(type: "bit", nullable: false),
                    otp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Ads",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<double>(type: "float", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    governorate = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    city = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    rentalFrequency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    area = table.Column<double>(type: "float", nullable: false),
                    bedrooms = table.Column<int>(type: "int", nullable: false),
                    bathrooms = table.Column<int>(type: "int", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    deposit = table.Column<int>(type: "int", nullable: false),
                    insurance = table.Column<int>(type: "int", nullable: false),
                    amenities = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    views = table.Column<int>(type: "int", nullable: false),
                    isRented = table.Column<bool>(type: "bit", nullable: false),
                    isDeleted = table.Column<bool>(type: "bit", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ads", x => x.id);
                    table.ForeignKey(
                        name: "FK_Ads_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    stripeTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    amount = table.Column<double>(type: "float", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false),
                    issuedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transactions", x => x.id);
                    table.ForeignKey(
                        name: "FK_Transactions_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdImages",
                columns: table => new
                {
                    imageUrl = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    adId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdImages", x => x.imageUrl);
                    table.ForeignKey(
                        name: "FK_AdImages_Ads_adId",
                        column: x => x.adId,
                        principalTable: "Ads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdReports",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    adId = table.Column<int>(type: "int", nullable: false),
                    subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    message = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdReports", x => x.id);
                    table.ForeignKey(
                        name: "FK_AdReports_Ads_adId",
                        column: x => x.adId,
                        principalTable: "Ads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdUserWishlists",
                columns: table => new
                {
                    userId = table.Column<int>(type: "int", nullable: false),
                    adId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdUserWishlists", x => new { x.userId, x.adId });
                    table.ForeignKey(
                        name: "FK_AdUserWishlists_Ads_adId",
                        column: x => x.adId,
                        principalTable: "Ads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdUserWishlists_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdImages_adId",
                table: "AdImages",
                column: "adId");

            migrationBuilder.CreateIndex(
                name: "IX_AdReports_adId",
                table: "AdReports",
                column: "adId");

            migrationBuilder.CreateIndex(
                name: "IX_Ads_userId",
                table: "Ads",
                column: "userId");

            migrationBuilder.CreateIndex(
                name: "IX_AdUserWishlists_adId",
                table: "AdUserWishlists",
                column: "adId");

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_userId",
                table: "Transactions",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdImages");

            migrationBuilder.DropTable(
                name: "AdReports");

            migrationBuilder.DropTable(
                name: "AdUserWishlists");

            migrationBuilder.DropTable(
                name: "Transactions");

            migrationBuilder.DropTable(
                name: "Ads");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
