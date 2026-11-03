using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberShipViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MemberShipsController : Controller
    {
        private readonly IMemberShipService _memberShipService;

        public MemberShipsController(IMemberShipService memberShipService)
        {
            _memberShipService = memberShipService;
        }

        // Display all memberships
        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var memberships = await _memberShipService.GetAllAsync(ct);
            return View(memberships);
        }

        // Open Create page
        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken ct)
        {
            var model = new MemberShipViewModel
            {
                StartDate = DateTime.Now,
                Members = await _memberShipService.GetMembersAsync(ct),
                Plans = await _memberShipService.GetActivePlansAsync(ct)
            };

            return View(model);
        }

        // Save new membership
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
            MemberShipViewModel model,
            CancellationToken ct)
        {
            if (!ModelState.IsValid)
            {
                model.Members = await _memberShipService.GetMembersAsync(ct);
                model.Plans = await _memberShipService.GetActivePlansAsync(ct);

                return View(model);
            }

            var result = await _memberShipService.CreateAsync(model, ct);

            if (!result)
            {
                ModelState.AddModelError(
                    string.Empty,
                    "Unable to create membership. Please check the selected member and plan.");

                model.Members = await _memberShipService.GetMembersAsync(ct);
                model.Plans = await _memberShipService.GetActivePlansAsync(ct);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
