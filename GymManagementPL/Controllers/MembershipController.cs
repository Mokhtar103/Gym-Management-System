using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class MembershipController : Controller
    {
        private readonly IMembershipService _membershipService;

        public MembershipController(IMembershipService membershipService)
        {
            _membershipService = membershipService;
        }
        public IActionResult Index()
        {
            var memberships = _membershipService.GetAllMemberships();
            return View(memberships);
        }

        public IActionResult Create()
        {
            LoadDropdowns();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateMembershipVM input)
        {
            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "Membership cannot be created, Check your data!";
                LoadDropdowns();
                return View(input);
               
            }
            var result = _membershipService.CreateMembership(input);
            if (result)
            {
                TempData["SuccessMessage"] = "Membership created successfully.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to create membership!";
                return View(input);
            }
        }

        public IActionResult Cancel(int id)
        {
            var result = _membershipService.DeleteMembership(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Membership cancelled successfully.";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed to delete membership!";
            }
            return RedirectToAction(nameof(Index));
        }
        #region Helper Methods
        public void LoadDropdowns()
        {
            var members = _membershipService.GetMembersForDropdown();
            var plans = _membershipService.GetPlansForDropdown();

            ViewBag.Members = new SelectList(members, "Id", "Name");
            ViewBag.Plans = new SelectList(plans, "Id", "Name");
        }
        #endregion
    }
}
