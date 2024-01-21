using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Account : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PenaltyId",
                table: "PlayerEntities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "AccountEntities",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Inventory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemId = table.Column<int>(type: "INTEGER", nullable: false),
                    itemAmount = table.Column<int>(type: "INTEGER", nullable: false),
                    Bounding = table.Column<byte>(type: "INTEGER", nullable: false),
                    PlayerEntityId = table.Column<int>(type: "INTEGER", nullable: true),
                    PlayerEntityId1 = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Inventory_PlayerEntities_PlayerEntityId",
                        column: x => x.PlayerEntityId,
                        principalTable: "PlayerEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Inventory_PlayerEntities_PlayerEntityId1",
                        column: x => x.PlayerEntityId1,
                        principalTable: "PlayerEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Penalty",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Penalty", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skill",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlayerEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skill", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Skill_PlayerEntities_PlayerEntityId",
                        column: x => x.PlayerEntityId,
                        principalTable: "PlayerEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEntities_PenaltyId",
                table: "PlayerEntities",
                column: "PenaltyId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_PlayerEntityId",
                table: "Inventory",
                column: "PlayerEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_PlayerEntityId1",
                table: "Inventory",
                column: "PlayerEntityId1");

            migrationBuilder.CreateIndex(
                name: "IX_Skill_PlayerEntityId",
                table: "Skill",
                column: "PlayerEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerEntities_Penalty_PenaltyId",
                table: "PlayerEntities",
                column: "PenaltyId",
                principalTable: "Penalty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerEntities_Penalty_PenaltyId",
                table: "PlayerEntities");

            migrationBuilder.DropTable(
                name: "Inventory");

            migrationBuilder.DropTable(
                name: "Penalty");

            migrationBuilder.DropTable(
                name: "Skill");

            migrationBuilder.DropIndex(
                name: "IX_PlayerEntities_PenaltyId",
                table: "PlayerEntities");

            migrationBuilder.DropColumn(
                name: "PenaltyId",
                table: "PlayerEntities");

            migrationBuilder.DropColumn(
                name: "Salt",
                table: "AccountEntities");
        }
    }
}
