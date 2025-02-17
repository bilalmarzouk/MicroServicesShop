﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Shop.Services.OrderApi.Migrations
{
    /// <inheritdoc />
    public partial class changepaymentintent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentId",
                table: "OrderHeaders",
                newName: "PaymentIntentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "PaymentIntentId",
                table: "OrderHeaders",
                newName: "PaymentId");
        }
    }
}
