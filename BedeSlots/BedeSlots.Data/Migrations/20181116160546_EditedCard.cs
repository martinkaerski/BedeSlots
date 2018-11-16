﻿using Microsoft.EntityFrameworkCore.Migrations;

namespace BedeSlots.Data.Migrations
{
    public partial class EditedCard : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankCards_CardTypes_TypeId",
                table: "BankCards");

            migrationBuilder.DropForeignKey(
                name: "FK_BankCards_AspNetUsers_UserId",
                table: "BankCards");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "Type",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "BankCards",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "CardTypes",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "IsDeleted", "ModifiedOn", "Name" },
                values: new object[,]
                {
                    { 1, null, null, false, null, "Visa" },
                    { 2, null, null, false, null, "MasterCard" },
                    { 3, null, null, false, null, "American Express" }
                });

            migrationBuilder.InsertData(
                table: "Currencies",
                columns: new[] { "Id", "CreatedOn", "DeletedOn", "IsDeleted", "ModifiedOn", "Name", "Symbol" },
                values: new object[,]
                {
                    { 2, null, null, false, null, "BGN", " " },
                    { 3, null, null, false, null, "EUR", " " },
                    { 4, null, null, false, null, "GBP", " " }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BankCards_CurrencyId",
                table: "BankCards",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_BankCards_Currencies_CurrencyId",
                table: "BankCards",
                column: "CurrencyId",
                principalTable: "Currencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BankCards_CardTypes_TypeId",
                table: "BankCards",
                column: "TypeId",
                principalTable: "CardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BankCards_AspNetUsers_UserId",
                table: "BankCards",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BankCards_Currencies_CurrencyId",
                table: "BankCards");

            migrationBuilder.DropForeignKey(
                name: "FK_BankCards_CardTypes_TypeId",
                table: "BankCards");

            migrationBuilder.DropForeignKey(
                name: "FK_BankCards_AspNetUsers_UserId",
                table: "BankCards");

            migrationBuilder.DropIndex(
                name: "IX_BankCards_CurrencyId",
                table: "BankCards");

            migrationBuilder.DeleteData(
                table: "CardTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CardTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CardTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Currencies",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "BankCards");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "AspNetUsers",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_BankCards_CardTypes_TypeId",
                table: "BankCards",
                column: "TypeId",
                principalTable: "CardTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_BankCards_AspNetUsers_UserId",
                table: "BankCards",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
