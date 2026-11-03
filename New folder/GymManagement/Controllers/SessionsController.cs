using GymManagement.BLL.Common;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SesisonViewModels;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagement.PL.Controllers
{
    public class SessionsController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionsController(ISessionService sessionService)
        {
            _sessionService=sessionService;
        }
        public async Task<IActionResult> Index()
        {
            var sessions =await _sessionService.GetAllSessionsAsync();
            return View(sessions);
        }

        #region Create

        [HttpGet]
        public async Task<IActionResult> Create()
        {

            await PopulateDropdownListAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateSessionViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            var result = await _sessionService.CreateSessionAsync(vm);


            if (result.success)
            {
                TempData["SuccessMessage"]="Session Created Successfully";
                return RedirectToAction(nameof(Index));

            }
           
            TempData["ErrorMessage"]=result.error;
            await PopulateDropdownListAsync();
            return View(vm);

        }


        #endregion
        [HttpGet]
        public async Task<IActionResult> Details(int id ,CancellationToken ct)
        {
            var result =await _sessionService.GetSessionByIdAsync(id,ct);
            if (result.success) return View(result.value);
            else
            {
                TempData["ErrorMessage"]=result.error;
                return RedirectToAction(nameof(Index));
            }
        }
        private async Task PopulateDropdownListAsync()
        {
            ViewBag.Trainers =new SelectList(await _sessionService.GetTrainersForDropdownAsync(), "Id", "Name");
            ViewBag.Categories =new SelectList(await _sessionService.GetCategoriesForDropdownAsync(), "Id", "Name");
        }


        #region Update

        [HttpGet]
        public async Task<IActionResult> Edit(int id ,CancellationToken ct)
        {
            var result = await _sessionService.GetSessionToUpdateAsync(id ,ct);
            if (result.success)
            {
                ViewBag.Trainers =new SelectList(await _sessionService.GetTrainersForDropdownAsync(), "Id", "Name");
                return View(result.value);

            }
            else
            {
                TempData["ErrorMessage"]=result.error;
                return RedirectToAction(nameof(Index));

            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id ,UpdateSessionViewModel model, CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Trainers =new SelectList(await _sessionService.GetTrainersForDropdownAsync(), "Id", "Name");
                return View(model);

            }

            var result = await _sessionService.UpdateSessionAsync(id, model, ct);

            if (result.success)
            {
                TempData["SuccessMessage"]="Session Updated Successfully";

                return RedirectToAction(nameof(Index));

            }
            else
            {
                TempData["ErrorMessage"]=result.error;
                ViewBag.Trainers =new SelectList(await _sessionService.GetTrainersForDropdownAsync(), "Id", "Name");
                return View(model);

            }

        }
        #endregion

        #region Delete


        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct) 
        {
            var result = await _sessionService.GetSessionByIdAsync(id);
            if (result.success) {
                return View();
            }
            else
            {
                TempData["ErrorMessage"]=result.error;
                return RedirectToAction(nameof(Index));
            }
        
        }

        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id, CancellationToken ct)
        {
            var res = await _sessionService.RemoveSessionAsync(id, ct);
            TempData[res.success ? "SuccessMessage" : "ErrorMessage"]= res.success ? "Session Deleted Successfully " : res.error;
            return RedirectToAction(nameof(Index));

        }

        #endregion

    }
}
