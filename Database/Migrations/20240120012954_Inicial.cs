using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AccountEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Login = table.Column<string>(type: "TEXT", nullable: false),
                    Password = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    LastLoginDate = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Position",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    MapNum = table.Column<int>(type: "INTEGER", nullable: false),
                    MapX = table.Column<int>(type: "INTEGER", nullable: false),
                    MapY = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Position", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Stat",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Strength = table.Column<int>(type: "INTEGER", nullable: false),
                    Endurance = table.Column<int>(type: "INTEGER", nullable: false),
                    Intelligence = table.Column<int>(type: "INTEGER", nullable: false),
                    Agility = table.Column<int>(type: "INTEGER", nullable: false),
                    WillPower = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stat", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vital",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    CurHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    CurEnergy = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxHealth = table.Column<int>(type: "INTEGER", nullable: false),
                    MaxEnergy = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vital", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerEntities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Sexo = table.Column<int>(type: "INTEGER", nullable: false),
                    ClassType = table.Column<int>(type: "INTEGER", nullable: false),
                    AccessType = table.Column<int>(type: "INTEGER", nullable: false),
                    Entidade = table.Column<int>(type: "INTEGER", nullable: false),
                    Sprite = table.Column<int>(type: "INTEGER", nullable: false),
                    Level = table.Column<int>(type: "INTEGER", nullable: false),
                    Exp = table.Column<int>(type: "INTEGER", nullable: false),
                    Points = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionId = table.Column<int>(type: "INTEGER", nullable: false),
                    StatId = table.Column<int>(type: "INTEGER", nullable: false),
                    VitalId = table.Column<int>(type: "INTEGER", nullable: false),
                    AccountEntityId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlayerEntities_AccountEntities_AccountEntityId",
                        column: x => x.AccountEntityId,
                        principalTable: "AccountEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerEntities_Position_PositionId",
                        column: x => x.PositionId,
                        principalTable: "Position",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerEntities_Stat_StatId",
                        column: x => x.StatId,
                        principalTable: "Stat",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PlayerEntities_Vital_VitalId",
                        column: x => x.VitalId,
                        principalTable: "Vital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEntities_AccountEntityId",
                table: "PlayerEntities",
                column: "AccountEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEntities_PositionId",
                table: "PlayerEntities",
                column: "PositionId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEntities_StatId",
                table: "PlayerEntities",
                column: "StatId");

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEntities_VitalId",
                table: "PlayerEntities",
                column: "VitalId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlayerEntities");

            migrationBuilder.DropTable(
                name: "AccountEntities");

            migrationBuilder.DropTable(
                name: "Position");

            migrationBuilder.DropTable(
                name: "Stat");

            migrationBuilder.DropTable(
                name: "Vital");
        }
    }
}
