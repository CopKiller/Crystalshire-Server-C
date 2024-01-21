using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class NewStructData2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_PlayerEntities_PlayerEntityId1",
                table: "Inventory");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_PlayerEntityId1",
                table: "Inventory");

            migrationBuilder.DropColumn(
                name: "PlayerEntityId1",
                table: "Inventory");

            migrationBuilder.CreateTable(
                name: "Bank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    ItemAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    Bounding = table.Column<byte>(type: "INTEGER", nullable: false),
                    PlayerEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bank", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bank_PlayerEntities_PlayerEntityId",
                        column: x => x.PlayerEntityId,
                        principalTable: "PlayerEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Bank_PlayerEntityId",
                table: "Bank",
                column: "PlayerEntityId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Bank");

            migrationBuilder.AddColumn<int>(
                name: "PlayerEntityId1",
                table: "Inventory",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_PlayerEntityId1",
                table: "Inventory",
                column: "PlayerEntityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_PlayerEntities_PlayerEntityId1",
                table: "Inventory",
                column: "PlayerEntityId1",
                principalTable: "PlayerEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
