using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DepositoDentalAPI.Migrations
{
    public partial class categoriaproductos : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CategoriaProducto_categorias_categoriaId",
                table: "CategoriaProducto");

            migrationBuilder.DropForeignKey(
                name: "FK_CategoriaProducto_productos_productoId",
                table: "CategoriaProducto");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoriaProducto",
                table: "CategoriaProducto");

            migrationBuilder.RenameTable(
                name: "CategoriaProducto",
                newName: "categoriaProductos");

            migrationBuilder.RenameIndex(
                name: "IX_CategoriaProducto_productoId",
                table: "categoriaProductos",
                newName: "IX_categoriaProductos_productoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_categoriaProductos",
                table: "categoriaProductos",
                columns: new[] { "categoriaId", "productoId" });

            migrationBuilder.AddForeignKey(
                name: "FK_categoriaProductos_categorias_categoriaId",
                table: "categoriaProductos",
                column: "categoriaId",
                principalTable: "categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_categoriaProductos_productos_productoId",
                table: "categoriaProductos",
                column: "productoId",
                principalTable: "productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_categoriaProductos_categorias_categoriaId",
                table: "categoriaProductos");

            migrationBuilder.DropForeignKey(
                name: "FK_categoriaProductos_productos_productoId",
                table: "categoriaProductos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_categoriaProductos",
                table: "categoriaProductos");

            migrationBuilder.RenameTable(
                name: "categoriaProductos",
                newName: "CategoriaProducto");

            migrationBuilder.RenameIndex(
                name: "IX_categoriaProductos_productoId",
                table: "CategoriaProducto",
                newName: "IX_CategoriaProducto_productoId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoriaProducto",
                table: "CategoriaProducto",
                columns: new[] { "categoriaId", "productoId" });

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriaProducto_categorias_categoriaId",
                table: "CategoriaProducto",
                column: "categoriaId",
                principalTable: "categorias",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CategoriaProducto_productos_productoId",
                table: "CategoriaProducto",
                column: "productoId",
                principalTable: "productos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
