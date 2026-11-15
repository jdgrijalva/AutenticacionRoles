
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AutenticacionRoles.Migrations
{
    public partial class RelacionGeneroPelicula : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Renombrar la duración sin perder los datos.
            migrationBuilder.RenameColumn(
                name: "Duracion",
                table: "Pelicula",
                newName: "DuracionMinutos");

            // Crear la tabla de géneros.
            migrationBuilder.CreateTable(
                name: "Genero",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(
                        type: "nvarchar(100)",
                        maxLength: 100,
                        nullable: false),
                    Descripcion = table.Column<string>(
                        type: "nvarchar(200)",
                        maxLength: 200,
                        nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Genero", x => x.Id);
                });

            // Agregar temporalmente la clave foránea como opcional.
            migrationBuilder.AddColumn<int>(
                name: "GeneroId",
                table: "Pelicula",
                type: "int",
                nullable: true);

            // Asignar un género a las películas que ya existan.
            migrationBuilder.Sql(@"
                IF EXISTS (SELECT 1 FROM [Pelicula]
                           WHERE [GeneroId] IS NULL)
                BEGIN
                    INSERT INTO [Genero] ([Nombre], [Descripcion])
                    VALUES (N'Sin clasificar',
                            N'Género asignado a películas existentes');

                    DECLARE @GeneroId int;
                    SET @GeneroId = SCOPE_IDENTITY();

                    UPDATE [Pelicula]
                    SET [GeneroId] = @GeneroId
                    WHERE [GeneroId] IS NULL;
                END
            ");

            // La clave foránea debe ser obligatoria.
            migrationBuilder.AlterColumn<int>(
                name: "GeneroId",
                table: "Pelicula",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            // Crear el índice de la relación.
            migrationBuilder.CreateIndex(
                name: "IX_Pelicula_GeneroId",
                table: "Pelicula",
                column: "GeneroId");

            // Establecer la relación uno a muchos.
            migrationBuilder.AddForeignKey(
                name: "FK_Pelicula_Genero_GeneroId",
                table: "Pelicula",
                column: "GeneroId",
                principalTable: "Genero",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Eliminar la relación.
            migrationBuilder.DropForeignKey(
                name: "FK_Pelicula_Genero_GeneroId",
                table: "Pelicula");

            // Eliminar el índice.
            migrationBuilder.DropIndex(
                name: "IX_Pelicula_GeneroId",
                table: "Pelicula");

            // Eliminar la clave foránea.
            migrationBuilder.DropColumn(
                name: "GeneroId",
                table: "Pelicula");

            // Eliminar la tabla de géneros.
            migrationBuilder.DropTable(
                name: "Genero");

            // Recuperar el nombre original de la duración.
            migrationBuilder.RenameColumn(
                name: "DuracionMinutos",
                table: "Pelicula",
                newName: "Duracion");
        }
    }
}
