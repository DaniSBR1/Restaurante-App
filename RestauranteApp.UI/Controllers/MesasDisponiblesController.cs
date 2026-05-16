using Microsoft.AspNetCore.Mvc;
using RestauranteApp.BL;
using RestauranteApp.UI.Models;

namespace RestauranteApp.UI.Controllers
{
    public class MesasDisponiblesController : Controller
    {
        private readonly MesaBL _mesaBL;

        public MesasDisponiblesController(MesaBL mesaBL) => _mesaBL = mesaBL;

        public async Task<IActionResult> Index(string? filtroNumero)
        {
            ViewBag.FiltroNumero = filtroNumero;
            var mesas = await _mesaBL.ObtenerDisponiblesFiltradoAsync(filtroNumero);
            return View(mesas);
        }

        public async Task<IActionResult> Reservar(int id)
        {
            var mesa = await _mesaBL.ObtenerPorIdAsync(id);
            if (mesa == null) return NotFound();

            var vm = new ReservaViewModel
            {
                MesaId       = mesa.Id,
                NumeroMesa   = mesa.NumeroMesa,
                Capacidad    = mesa.Capacidad,
                PrecioPorPersona = mesa.PrecioPorPersona
            };
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Reservar(ReservaViewModel vm)
        {
            if (!ModelState.IsValid)
                return View(vm);

            try
            {
                await _mesaBL.ReservarMesaAsync(
                    vm.MesaId, vm.IdentificacionCliente!, vm.NombreCliente!,
                    vm.FechaReserva, vm.CantidadComensales);

                TempData["Mensaje"] = "Mesa reservada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }
    }
}
