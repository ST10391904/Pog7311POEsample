using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Prog7311PoeTwo.Migrations
{
    /// <inheritdoc />
    public partial class AddContractFileUpload : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Contracts",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Contracts");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Contracts");
        }
    }
}
