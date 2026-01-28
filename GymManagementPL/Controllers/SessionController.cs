using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GymManagementPL.Controllers
{
    public class SessionController : Controller
    {
        private readonly ISessionService _sessionService;

        public SessionController(ISessionService sessionService)
        {
            _sessionService = sessionService;
        }
        public IActionResult Index()
        {
            var sessions = _sessionService.GetAllSessions();
            return View(sessions);
        }

        public IActionResult Create()
        {
            LoadCategories();
            LoadTrainers();
            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateSessionVM input)
        {
            if (!ModelState.IsValid)
            {
                LoadCategories();
                LoadTrainers();
                return View(input);
            }

            var result = _sessionService.CreateSession(input);

            if (result)
            {
                TempData["SuccessMessage"] = "Session created successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "An error occurred while creating the session.");
                return View(input);

            }
        }

        public IActionResult Details(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction(nameof(Index));
            }

            var session = _sessionService.GetSessionById(id);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(session);
        }

        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                TempData["ErrorMessage"] = "Invalid session ID.";
                return RedirectToAction(nameof(Index));
            }
            var session = _sessionService.GetSessionToUpdate(id);

            if (session is null)
            {
                TempData["ErrorMessage"] = "Session not found.";
                return RedirectToAction(nameof(Index));
            }

            LoadTrainers();
            return View(session);
        }

        [HttpPost]
        public IActionResult Edit([FromRoute] int id, UpdateSessionVM input)
        {
            if (!ModelState.IsValid)
            {
                LoadTrainers();
                return View(input);
            }
            var result = _sessionService.UpdateSession(id, input);
            if (result)
            {
                TempData["SuccessMessage"] = "Session updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ModelState.AddModelError("", "An error occurred while updating the session.");
                return View(input);
            }
        }

        [HttpPost]
        public IActionResult DeleteConfirmed([FromForm] int id)
        {
            bool result = _sessionService.RemoveSession(id);

            if (result)
                TempData["SuccessMessage"] = "Session Deleted Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Delete Session";

            return RedirectToAction(nameof(Index));
        }

        #region Helper Methods

        public void LoadCategories()
        {
            var categories = _sessionService.GetCategoriesDropDown();
            ViewBag.Categories = new SelectList(categories, "Id", "Name");
        }

        public void LoadTrainers()
        {
            var trainers = _sessionService.GetTrainersDropDown();
            ViewBag.Trainers = new SelectList(trainers, "Id", "Name");
        }

        #endregion
    }
}
