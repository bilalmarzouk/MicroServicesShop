using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Services.OrderApi.Migrations
{
    /// <inheritdoc />
    public partial class fixcoloumnname : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrdertHeaderId",
                table: "OrderHeaders",
                newName: "OrderHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrderHeaderId",
                table: "OrderHeaders",
                newName: "OrdertHeaderId");
        }
    }
}
