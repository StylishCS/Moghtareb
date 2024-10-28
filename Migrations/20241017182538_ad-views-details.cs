using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Moghtareb.Migrations
{
    /// <inheritdoc />
    public partial class adviewsdetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "views",
                table: "Ads");

            migrationBuilder.CreateTable(
                name: "AdUserPhoneViews",
                columns: table => new
                {
                    adId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdUserPhoneViews", x => new { x.userId, x.adId });
                    table.ForeignKey(
                        name: "FK_AdUserPhoneViews_Ads_adId",
                        column: x => x.adId,
                        principalTable: "Ads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdUserPhoneViews_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AdUserViews",
                columns: table => new
                {
                    adId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdUserViews", x => new { x.userId, x.adId });
                    table.ForeignKey(
                        name: "FK_AdUserViews_Ads_adId",
                        column: x => x.adId,
                        principalTable: "Ads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdUserViews_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "AdUserWhatsappViews",
                columns: table => new
                {
                    adId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdUserWhatsappViews", x => new { x.userId, x.adId });
                    table.ForeignKey(
                        name: "FK_AdUserWhatsappViews_Ads_adId",
                        column: x => x.adId,
                        principalTable: "Ads",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AdUserWhatsappViews_Users_userId",
                        column: x => x.userId,
                        principalTable: "Users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AdUserPhoneViews_adId",
                table: "AdUserPhoneViews",
                column: "adId");

            migrationBuilder.CreateIndex(
                name: "IX_AdUserViews_adId",
                table: "AdUserViews",
                column: "adId");

            migrationBuilder.CreateIndex(
                name: "IX_AdUserWhatsappViews_adId",
                table: "AdUserWhatsappViews",
                column: "adId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdUserPhoneViews");

            migrationBuilder.DropTable(
                name: "AdUserViews");

            migrationBuilder.DropTable(
                name: "AdUserWhatsappViews");

            migrationBuilder.AddColumn<int>(
                name: "views",
                table: "Ads",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
