using GymManagement.DAL.Models;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.Controllers
{
    public class PlansController : Controller
    {
        private readonly IGenericRepository<Plan> _planRepository;

        public PlansController(IGenericRepository<Plan> planRepository)
        {
            _planRepository = planRepository;
        }

        public async Task<IActionResult> Index(CancellationToken ct)
        {
            var plans = await _planRepository.GetAllAsync(ct: ct);

            return View(plans);
        }

        public async Task<IActionResult> Details(int id, CancellationToken ct)
        {
            var plan = await _planRepository.GetByIdAsync(id, ct);

            if (plan is null)
                return RedirectToAction(nameof(Index));

            return View(plan);
        }
    }
}