using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class NovoModelo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Position_PlayerEntities_Id",
                table: "Position");

            migrationBuilder.DropForeignKey(
                name: "FK_Stat_PlayerEntities_Id",
                table: "Stat");

            migrationBuilder.DropForeignKey(
                name: "FK_Vital_PlayerEntities_Id",
                table: "Vital");

            migrationBuilder.AddColumn<int>(
                name: "PositionId",
                table: "PlayerEntities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "StatId",
                table: "PlayerEntities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "VitalId",
                table: "PlayerEntities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerEntities_Position_PositionId",
                table: "PlayerEntities",
                column: "PositionId",
                principalTable: "Position",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerEntities_Stat_StatId",
                table: "PlayerEntities",
                column: "StatId",
                principalTable: "Stat",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerEntities_Vital_VitalId",
                table: "PlayerEntities",
                column: "VitalId",
                principalTable: "Vital",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerEntities_Position_PositionId",
                table: "PlayerEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerEntities_Stat_StatId",
                table: "PlayerEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_PlayerEntities_Vital_VitalId",
                table: "PlayerEntities");

            migrationBuilder.DropIndex(
                name: "IX_PlayerEntities_PositionId",
                table: "PlayerEntities");

            migrationBuilder.DropIndex(
                name: "IX_PlayerEntities_StatId",
                table: "PlayerEntities");

            migrationBuilder.DropIndex(
                name: "IX_PlayerEntities_VitalId",
                table: "PlayerEntities");

            migrationBuilder.DropColumn(
                name: "PositionId",
                table: "PlayerEntities");

            migrationBuilder.DropColumn(
                name: "StatId",
                table: "PlayerEntities");

            migrationBuilder.DropColumn(
                name: "VitalId",
                table: "PlayerEntities");

            migrationBuilder.AddForeignKey(
                name: "FK_Position_PlayerEntities_Id",
                table: "Position",
                column: "Id",
                principalTable: "PlayerEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stat_PlayerEntities_Id",
                table: "Stat",
                column: "Id",
                principalTable: "PlayerEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vital_PlayerEntities_Id",
                table: "Vital",
                column: "Id",
                principalTable: "PlayerEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
