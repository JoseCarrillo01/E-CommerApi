using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepositoDentalAPI.Migrations
{
    public partial class PedidosEliminado : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "eliminado",
                table: "pedidostabla",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "eliminado",
                table: "pedidostabla");
        }
    }
}
