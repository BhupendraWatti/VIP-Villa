using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Villa_Services.Migrations
{
    /// <inheritdoc />
    public partial class addnewTableForeignkeyToVillaNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "VillaId",
                table: "VillaNo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_VillaNo_VillaId",
                table: "VillaNo",
                column: "VillaId");

            migrationBuilder.AddForeignKey(
                name: "FK_VillaNo_Villas_VillaId",
                table: "VillaNo",
                column: "VillaId",
                principalTable: "Villas",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VillaNo_Villas_VillaId",
                table: "VillaNo");

            migrationBuilder.DropIndex(
                name: "IX_VillaNo_VillaId",
                table: "VillaNo");

            migrationBuilder.DropColumn(
                name: "VillaId",
                table: "VillaNo");
        }
    }
}
