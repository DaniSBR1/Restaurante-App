using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestauranteApp.Model
{
    public class Mesa
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int NumeroMesa { get; set; }

        public int Estado { get; set; } = (int)EstadoMesa.Disponible;

        [Required]
        public int Capacidad { get; set; }

        [Required]
        public string Seccion { get; set; } = string.Empty;

        [Required]
        [Column(TypeName = "money")]
        public decimal PrecioPorPersona { get; set; }

        public string? IdentificacionCliente { get; set; }
        public string? NombreCliente { get; set; }
        public DateOnly? FechaReserva { get; set; }
        public int? CantidadComensales { get; set; }

        [Column(TypeName = "money")]
        public decimal? DepositoDeGarantia { get; set; }
    }
}
