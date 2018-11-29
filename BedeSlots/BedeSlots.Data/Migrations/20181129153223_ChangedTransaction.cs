using Microsoft.EntityFrameworkCore.Migrations;

namespace BedeSlots.Data.Migrations
{
    public partial class ChangedTransaction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GameType",
                table: "Transactions",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "GameType",
                table: "Transactions",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
