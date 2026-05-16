using Microsoft.AspNetCore.Mvc;
using RestauranteApp.BL;
using RestauranteApp.Model;

namespace RestauranteApp.UI.Controllers
{
    /// <summary>
    /// Módulo: Registro de Mesas.
    /// Expone el CRUD completo sobre la entidad Mesa.
    /// Esta capa no contiene lógica de negocio; delega todo a MesaBL.
    /// </summary>
    public class MesasController : Controller
    {
        private readonly MesaBL _mesaBL;

        public MesasController(MesaBL mesaBL) => _mesaBL = mesaBL;

        // ── READ: lista completa con filtro opcional ──────────────────────────────

        public async Task<IActionResult> Index(string? filtroNumero)
        {
            ViewBag.FiltroNumero = filtroNumero;
            var mesas = await _mesaBL.ObtenerTodasFiltradoAsync(filtroNumero);
            return View(mesas);
        }

        // ── CREATE ────────────────────────────────────────────────────────────────

        public IActionResult Agregar() => View(new Mesa());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Agregar(Mesa mesa)
        {
            if (!ModelState.IsValid)
                return View(mesa);

            try
            {
                await _mesaBL.AgregarMesaAsync(mesa);
                TempData["Mensaje"] = "Mesa agregada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(mesa);
            }
        }

        // ── UPDATE ────────────────────────────────────────────────────────────────

        public async Task<IActionResult> Editar(int id)
        {
            var mesa = await _mesaBL.ObtenerPorIdAsync(id);
            if (mesa == null) return NotFound();
            return View(mesa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Editar(Mesa mesa)
        {
            if (!ModelState.IsValid)
                return View(mesa);

            try
            {
                await _mesaBL.EditarMesaAsync(mesa);
                TempData["Mensaje"] = "Mesa actualizada exitosamente.";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(mesa);
            }
        }

        // ── DELETE ────────────────────────────────────────────────────────────────

        public async Task<IActionResult> Eliminar(int id)
        {
            var mesa = await _mesaBL.ObtenerPorIdAsync(id);
            if (mesa == null) return NotFound();
            return View(mesa);
        }

        [HttpPost, ActionName("Eliminar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmarEliminar(int id)
        {
            await _mesaBL.EliminarMesaAsync(id);
            TempData["Mensaje"] = "Mesa eliminada exitosamente.";
            return RedirectToAction(nameof(Index));
        }
    }
}
