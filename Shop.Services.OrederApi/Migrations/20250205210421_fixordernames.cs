using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Services.OrderApi.Migrations
{
    /// <inheritdoc />
    public partial class fixordernames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ProductHeaderId",
                table: "OrderHeaders",
                newName: "OrdertHeaderId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "OrdertHeaderId",
                table: "OrderHeaders",
                newName: "ProductHeaderId");
        }
    }
}
