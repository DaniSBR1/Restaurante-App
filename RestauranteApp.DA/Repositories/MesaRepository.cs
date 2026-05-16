using Microsoft.EntityFrameworkCore;
using RestauranteApp.DA.Context;
using RestauranteApp.Model;

namespace RestauranteApp.DA.Repositories
{
    /// <summary>
    /// Implementación concreta del repositorio de Mesa usando Entity Framework Core.
    /// Contiene todas las operaciones CRUD y consultas específicas de la entidad.
    /// </summary>
    public class MesaRepository : IMesaRepository
    {
        private readonly RestauranteContext _context;

        public MesaRepository(RestauranteContext context)
        {
            _context = context;
        }

        // ── CREATE ──────────────────────────────────────────────────────────────

        public async Task AddAsync(Mesa entity)
        {
            await _context.Mesas.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        // ── READ ─────────────────────────────────────────────────────────────────

        public async Task<IEnumerable<Mesa>> GetAllAsync()
        {
            return await _context.Mesas
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Mesa?> GetByIdAsync(int id)
        {
            return await _context.Mesas.FindAsync(id);
        }

        public async Task<IEnumerable<Mesa>> GetByEstadoAsync(EstadoMesa estado)
        {
            return await _context.Mesas
                .AsNoTracking()
                .Where(m => m.Estado == (int)estado)
                .ToListAsync();
        }

        public async Task<IEnumerable<Mesa>> GetByNumeroMesaAsync(int numeroMesa)
        {
            return await _context.Mesas
                .AsNoTracking()
                .Where(m => m.NumeroMesa == numeroMesa)
                .ToListAsync();
        }

        public async Task<bool> ExistsNumeroMesaAsync(int numeroMesa, int? excludeId = null)
        {
            return await _context.Mesas
                .AnyAsync(m => m.NumeroMesa == numeroMesa && (!excludeId.HasValue || m.Id != excludeId.Value));
        }

        public async Task<IEnumerable<Mesa>> GetReservadasByNombreClienteAsync(string nombreCliente)
        {
            return await _context.Mesas
                .AsNoTracking()
                .Where(m => m.Estado == (int)EstadoMesa.Reservada &&
                            m.NombreCliente != null &&
                            m.NombreCliente.Contains(nombreCliente))
                .ToListAsync();
        }

        // ── UPDATE ───────────────────────────────────────────────────────────────

        public async Task UpdateAsync(Mesa entity)
        {
            _context.Mesas.Update(entity);
            await _context.SaveChangesAsync();
        }

        // ── DELETE ───────────────────────────────────────────────────────────────

        public async Task DeleteAsync(int id)
        {
            var mesa = await _context.Mesas.FindAsync(id);
            if (mesa != null)
            {
                _context.Mesas.Remove(mesa);
                await _context.SaveChangesAsync();
            }
        }
    }
}
