using Microsoft.EntityFrameworkCore.Migrations;

namespace BedeSlots.Data.Migrations
{
    public partial class EditedBankCard : Migration
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

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "BankCards");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "AspNetUsers");

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
