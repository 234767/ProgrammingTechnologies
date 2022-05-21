using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.SampleImpl.Migrations
{
    public partial class Return_Lease_Relation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LeaseDtoId",
                table: "Returns",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Returns_LeaseDtoId",
                table: "Returns",
                column: "LeaseDtoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Leases_LeaseDtoId",
                table: "Returns",
                column: "LeaseDtoId",
                principalTable: "Leases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Leases_LeaseDtoId",
                table: "Returns");

            migrationBuilder.DropIndex(
                name: "IX_Returns_LeaseDtoId",
                table: "Returns");

            migrationBuilder.DropColumn(
                name: "LeaseDtoId",
                table: "Returns");
        }
    }
}
