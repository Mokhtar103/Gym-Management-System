using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class PlanController : Controller
    {
        private readonly IPlanService _planService;

        public PlanController(IPlanService planService)
        {
            _planService = planService;
        }
        public IActionResult Index()
        {
            var plans = _planService.GetAllPlans();
            return View(plans);
        }
        public IActionResult PlanDetails(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid Plan Id!";
                return RedirectToAction(nameof(Index));
            }

            var plan = _planService.GetPlanById(id);

            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);

        }

        public IActionResult PlanEdit(int id)
        {
            var plan = _planService.GetPlanToUpdate(id);
            if (plan is null)
            {
                TempData["ErrorMessage"] = "Plan Not Found!";
                return RedirectToAction(nameof(Index));
            }
            return View(plan);
        }

        [HttpPost]
        public IActionResult PlanEdit([FromRoute] int id, UpdatedPlanVM input)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("WrongData", "Check Data Validation!");
                return View(input);
            }

            bool result = _planService.UpdatePlan(id, input);

            if (result)
                TempData["SuccessMessage"] = "Plan Updated Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Update Plan";

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult PlanActivate(int id)
        {
            bool result = _planService.ActivatePlan(id);

            if (result)
                TempData["SuccessMessage"] = "Plan Activated Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Activate Plan";

            return RedirectToAction(nameof(Index));
        }
    }
}
