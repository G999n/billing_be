using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace billing_be.Migrations
{
    /// <inheritdoc />
    public partial class AddDrugLicenseColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DrugLicense",
                table: "Clients",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DrugLicense",
                table: "Clients");
        }
    }
}
