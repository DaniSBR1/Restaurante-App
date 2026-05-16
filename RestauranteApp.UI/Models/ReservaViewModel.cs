using System.ComponentModel.DataAnnotations;

namespace RestauranteApp.UI.Models
{
    public class ReservaViewModel
    {
        public int  MesaId     { get; set; }
        public int  NumeroMesa { get; set; }
        public int  Capacidad  { get; set; }
        public decimal PrecioPorPersona { get; set; }

        [Required(ErrorMessage = "La identificación del cliente es requerida.")]
        public string? IdentificacionCliente { get; set; }

        [Required(ErrorMessage = "El nombre del cliente es requerido.")]
        public string? NombreCliente { get; set; }

        [Required(ErrorMessage = "La fecha de la reserva es requerida.")]
        public DateOnly FechaReserva { get; set; }

        [Required(ErrorMessage = "La cantidad de comensales es requerida.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad de comensales debe ser mayor a cero.")]
        public int CantidadComensales { get; set; }
    }
}
