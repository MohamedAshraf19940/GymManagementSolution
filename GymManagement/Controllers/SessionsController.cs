
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SesisonViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var sessions = await _sessionService.GetAllSessionsAsync(ct);
            return View(sessions);
        }

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            await PopulateDropdownListAsync(ct);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            CreateSessionViewModel vm,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDropdownListAsync(ct);
                return View(vm);
            }

            var result = await _sessionService.CreateSessionAsync(vm, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Created Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;

            await PopulateDropdownListAsync(ct);
            return View(vm);
        }

        #endregion

        #region Details

        [HttpGet]
        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionByIdAsync(id, ct);

            if (result.success)
                return View(result.value);

            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }

        #endregion

        #region Dropdown Lists

        private async Task PopulateDropdownListAsync(CancellationToken ct)
        {
            var trainers = await _sessionService.GetTrainersForDropdownAsync(ct);
            var categories = await _sessionService.GetCategoriesForDropdownAsync(ct);

            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }

        #endregion

        #region Update

        [HttpGet]
        public async Task<IActionResult> Edit(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionToUpdateAsync(id, ct);

            if (result.success)
            {
                ViewBag.Trainers = new SelectList(
                    await _sessionService.GetTrainersForDropdownAsync(ct),
                    "Id",
                    "Name");

                return View(result.value);
            }

            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(
            int id,
            UpdateSessionViewModel model,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Trainers = new SelectList(
                    await _sessionService.GetTrainersForDropdownAsync(ct),
                    "Id",
                    "Name");

                return View(model);
            }

            var result = await _sessionService.UpdateSessionAsync(id, model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"] = "Session Updated Successfully";
                return RedirectToAction(nameof(Index));
            }

            TempData["ErrorMessage"] = result.error;

            ViewBag.Trainers = new SelectList(
                await _sessionService.GetTrainersForDropdownAsync(ct),
                "Id",
                "Name");

            return View(model);
        }

        #endregion

        #region Delete

        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var result = await _sessionService.GetSessionByIdAsync(id, ct);

            if (result.success)
                return View();

            TempData["ErrorMessage"] = result.error;
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var result = await _sessionService.RemoveSessionAsync(id, ct);

            TempData[result.success ? "SuccessMessage" : "ErrorMessage"] =
                result.success
                    ? "Session Deleted Successfully"
                    : result.error;

            return RedirectToAction(nameof(Index));
        }

        #endregion
    }
}
