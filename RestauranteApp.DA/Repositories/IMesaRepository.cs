using RestauranteApp.Model;

namespace RestauranteApp.DA.Repositories
{
    /// <summary>
    /// Interfaz específica del repositorio de Mesa.
    /// Extiende IRepository con consultas propias del dominio de mesas.
    /// </summary>
    public interface IMesaRepository : IRepository<Mesa>
    {
        Task<IEnumerable<Mesa>> GetByEstadoAsync(EstadoMesa estado);
        Task<IEnumerable<Mesa>> GetByNumeroMesaAsync(int numeroMesa);
        Task<IEnumerable<Mesa>> GetReservadasByNombreClienteAsync(string nombreCliente);
        /// <summary>Verifica si ya existe una mesa con ese número (excluyendo el id dado para edición).</summary>
        Task<bool> ExistsNumeroMesaAsync(int numeroMesa, int? excludeId = null);
    }
}
