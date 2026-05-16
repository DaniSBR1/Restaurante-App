using RestauranteApp.DA.Repositories;
using RestauranteApp.Model;

namespace RestauranteApp.BL
{
    /// <summary>
    /// Capa de lógica de negocio para la entidad Mesa.
    /// Todos los cálculos y validaciones de negocio residen aquí.
    /// </summary>
    public class MesaBL
    {
        private readonly IMesaRepository _mesaRepository;

        public MesaBL(IMesaRepository mesaRepository)
        {
            _mesaRepository = mesaRepository;
        }

        // ── Módulo Registro de Mesas ─────────────────────────────────────────────

        public async Task<IEnumerable<Mesa>> ObtenerTodasAsync()
            => await _mesaRepository.GetAllAsync();

        public async Task<IEnumerable<Mesa>> ObtenerTodasFiltradoAsync(string? filtroNumero)
        {
            if (!string.IsNullOrWhiteSpace(filtroNumero) && int.TryParse(filtroNumero, out int numero))
                return await _mesaRepository.GetByNumeroMesaAsync(numero);
            return await _mesaRepository.GetAllAsync();
        }

        public async Task<Mesa?> ObtenerPorIdAsync(int id)
            => await _mesaRepository.GetByIdAsync(id);

        public async Task AgregarMesaAsync(Mesa mesa)
        {
            if (await _mesaRepository.ExistsNumeroMesaAsync(mesa.NumeroMesa))
                throw new InvalidOperationException($"Ya existe una mesa con el número {mesa.NumeroMesa}. Use un número diferente.");

            if (mesa.Capacidad <= 0)
                throw new InvalidOperationException("La capacidad debe ser mayor a cero.");

            if (mesa.PrecioPorPersona <= 0)
                throw new InvalidOperationException("El precio por persona debe ser mayor a cero.");

            mesa.Estado = (int)EstadoMesa.Disponible;
            await _mesaRepository.AddAsync(mesa);
        }

        public async Task EditarMesaAsync(Mesa mesaActualizada)
        {
            var mesaExistente = await _mesaRepository.GetByIdAsync(mesaActualizada.Id)
                ?? throw new InvalidOperationException("Mesa no encontrada.");

            if (await _mesaRepository.ExistsNumeroMesaAsync(mesaActualizada.NumeroMesa, mesaActualizada.Id))
                throw new InvalidOperationException($"Ya existe otra mesa con el número {mesaActualizada.NumeroMesa}. Use un número diferente.");

            if (mesaActualizada.Capacidad <= 0)
                throw new InvalidOperationException("La capacidad debe ser mayor a cero.");

            if (mesaActualizada.PrecioPorPersona <= 0)
                throw new InvalidOperationException("El precio por persona debe ser mayor a cero.");

            mesaExistente.NumeroMesa       = mesaActualizada.NumeroMesa;
            mesaExistente.Capacidad        = mesaActualizada.Capacidad;
            mesaExistente.Seccion          = mesaActualizada.Seccion;
            mesaExistente.PrecioPorPersona = mesaActualizada.PrecioPorPersona;

            await _mesaRepository.UpdateAsync(mesaExistente);
        }

        public async Task EliminarMesaAsync(int id)
            => await _mesaRepository.DeleteAsync(id);

        // ── Módulo Mesas Disponibles ─────────────────────────────────────────────

        public async Task<IEnumerable<Mesa>> ObtenerDisponiblesFiltradoAsync(string? filtroNumero)
        {
            var mesas = await _mesaRepository.GetByEstadoAsync(EstadoMesa.Disponible);
            if (!string.IsNullOrWhiteSpace(filtroNumero) && int.TryParse(filtroNumero, out int numero))
                return mesas.Where(m => m.NumeroMesa == numero);
            return mesas;
        }

        public async Task ReservarMesaAsync(int mesaId, string identificacionCliente,
            string nombreCliente, DateOnly fechaReserva, int cantidadComensales)
        {
            var mesa = await _mesaRepository.GetByIdAsync(mesaId)
                ?? throw new InvalidOperationException("Mesa no encontrada.");

            if (mesa.Estado != (int)EstadoMesa.Disponible)
                throw new InvalidOperationException("La mesa seleccionada ya no está disponible.");

            if (fechaReserva < DateOnly.FromDateTime(DateTime.Today))
                throw new InvalidOperationException("La fecha de la reserva no puede ser en el pasado.");

            if (cantidadComensales <= 0)
                throw new InvalidOperationException("La cantidad de comensales debe ser mayor a cero.");

            if (cantidadComensales > mesa.Capacidad)
                throw new InvalidOperationException(
                    $"La cantidad de comensales ({cantidadComensales}) supera la capacidad de la mesa ({mesa.Capacidad} personas).");

            mesa.IdentificacionCliente = identificacionCliente.Trim();
            mesa.NombreCliente         = nombreCliente.Trim();
            mesa.FechaReserva          = fechaReserva;
            mesa.CantidadComensales    = cantidadComensales;
            mesa.DepositoDeGarantia    = CalcularDepositoGarantia(mesa.PrecioPorPersona, cantidadComensales);
            mesa.Estado                = (int)EstadoMesa.Reservada;

            await _mesaRepository.UpdateAsync(mesa);
        }

        // ── Módulo Mesas Reservadas ──────────────────────────────────────────────

        public async Task<IEnumerable<Mesa>> ObtenerReservadasFiltradoAsync(string? filtroCliente)
        {
            if (!string.IsNullOrWhiteSpace(filtroCliente))
                return await _mesaRepository.GetReservadasByNombreClienteAsync(filtroCliente);
            return await _mesaRepository.GetByEstadoAsync(EstadoMesa.Reservada);
        }

        public async Task LiberarMesaAsync(int mesaId)
        {
            var mesa = await _mesaRepository.GetByIdAsync(mesaId)
                ?? throw new InvalidOperationException("Mesa no encontrada.");

            if (mesa.Estado != (int)EstadoMesa.Reservada)
                throw new InvalidOperationException("La mesa ya se encuentra disponible.");

            mesa.Estado                = (int)EstadoMesa.Disponible;
            mesa.IdentificacionCliente = null;
            mesa.NombreCliente         = null;
            mesa.FechaReserva          = null;
            mesa.CantidadComensales    = null;
            mesa.DepositoDeGarantia    = null;

            await _mesaRepository.UpdateAsync(mesa);
        }

        // ── Cálculos (residen exclusivamente en BL) ──────────────────────────────

        public decimal CalcularDepositoGarantia(decimal precioPorPersona, int cantidadComensales)
        {
            if (cantidadComensales < 4)
                return precioPorPersona * cantidadComensales;          // 100 %
            if (cantidadComensales < 8)
                return precioPorPersona * cantidadComensales * 0.75m; // 75 %
            return precioPorPersona * cantidadComensales * 0.50m;     // 50 %
        }
    }
}
