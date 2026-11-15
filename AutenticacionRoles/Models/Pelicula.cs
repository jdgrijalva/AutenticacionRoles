
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace AutenticacionRoles.Models
{
    public class Pelicula
    {
        [Display(Name = "Código")]
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre de la película es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar los 150 caracteres.")]
        [Display(Name = "Nombre de la película")]
        public string Titulo { get; set; } = "";

        [Required(ErrorMessage = "El nombre del director es obligatorio.")]
        [StringLength(50, ErrorMessage = "El nombre del director no puede superar los 50 caracteres.")]
        [Display(Name = "Director")]
        public string Director { get; set; } = "";

        // Duración de la película en minutos.
        [Range(1, 500, ErrorMessage = "La duración debe estar entre 1 y 500 minutos.")]
        [Display(Name = "Duración (minutos)")]
        public int DuracionMinutos { get; set; }

        // Clave foránea que relaciona la película con su género.
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar un género.")]
        [Display(Name = "Género")]
        public int GeneroId { get; set; }

        // Permite consultar los datos del género.
        [ValidateNever]
        public Genero Genero { get; set; } = null!;
    }
}
