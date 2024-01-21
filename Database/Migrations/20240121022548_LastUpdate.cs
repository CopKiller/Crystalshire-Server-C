using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Database.Migrations
{
    /// <inheritdoc />
    public partial class LastUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PlayerEntities_Penalty_PenaltyId",
                table: "PlayerEntities");

            migrationBuilder.DropIndex(
                name: "IX_PlayerEntities_PenaltyId",
                table: "PlayerEntities");

            migrationBuilder.DropColumn(
                name: "PenaltyId",
                table: "PlayerEntities");

            migrationBuilder.AddColumn<int>(
                name: "SkillId",
                table: "Skill",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "SkillUses",
                table: "Skill",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "isBanned",
                table: "Penalty",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "isMuted",
                table: "Penalty",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddForeignKey(
                name: "FK_Penalty_PlayerEntities_Id",
                table: "Penalty",
                column: "Id",
                principalTable: "PlayerEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Penalty_PlayerEntities_Id",
                table: "Penalty");

            migrationBuilder.DropColumn(
                name: "SkillId",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "SkillUses",
                table: "Skill");

            migrationBuilder.DropColumn(
                name: "isBanned",
                table: "Penalty");

            migrationBuilder.DropColumn(
                name: "isMuted",
                table: "Penalty");

            migrationBuilder.AddColumn<int>(
                name: "PenaltyId",
                table: "PlayerEntities",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_PlayerEntities_PenaltyId",
                table: "PlayerEntities",
                column: "PenaltyId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlayerEntities_Penalty_PenaltyId",
                table: "PlayerEntities",
                column: "PenaltyId",
                principalTable: "Penalty",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
