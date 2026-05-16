using Microsoft.AspNetCore.Mvc;
using RestauranteApp.BL;

namespace RestauranteApp.UI.Controllers
{
    public class MesasReservadasController : Controller
    {
        private readonly MesaBL _mesaBL;

        public MesasReservadasController(MesaBL mesaBL) => _mesaBL = mesaBL;

        public async Task<IActionResult> Index(string? filtroCliente)
        {
            ViewBag.FiltroCliente = filtroCliente;
            var mesas = await _mesaBL.ObtenerReservadasFiltradoAsync(filtroCliente);
            return View(mesas);
        }

        public async Task<IActionResult> Detalle(int id)
        {
            var mesa = await _mesaBL.ObtenerPorIdAsync(id);
            if (mesa == null) return NotFound();
            return View(mesa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Liberar(int id)
        {
            try
            {
                await _mesaBL.LiberarMesaAsync(id);
                TempData["Mensaje"] = "Mesa liberada exitosamente.";
            }
            catch (InvalidOperationException ex)
            {
                TempData["Error"] = ex.Message;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
