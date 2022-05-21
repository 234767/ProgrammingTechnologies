using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.SampleImpl.Migrations
{
    public partial class NameChange : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Leases_LeaseDtoId",
                table: "Returns");

            migrationBuilder.RenameColumn(
                name: "LeaseDtoId",
                table: "Returns",
                newName: "LeaseId");

            migrationBuilder.RenameIndex(
                name: "IX_Returns_LeaseDtoId",
                table: "Returns",
                newName: "IX_Returns_LeaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Leases_LeaseId",
                table: "Returns",
                column: "LeaseId",
                principalTable: "Leases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Returns_Leases_LeaseId",
                table: "Returns");

            migrationBuilder.RenameColumn(
                name: "LeaseId",
                table: "Returns",
                newName: "LeaseDtoId");

            migrationBuilder.RenameIndex(
                name: "IX_Returns_LeaseId",
                table: "Returns",
                newName: "IX_Returns_LeaseDtoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Returns_Leases_LeaseDtoId",
                table: "Returns",
                column: "LeaseDtoId",
                principalTable: "Leases",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
