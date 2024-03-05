using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FribergCarRentalsBravo.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Renamedcategorynamefieldincarcategoryentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Name",
                table: "CarCategories",
                newName: "CategoryName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CategoryName",
                table: "CarCategories",
                newName: "Name");
        }
    }
}
