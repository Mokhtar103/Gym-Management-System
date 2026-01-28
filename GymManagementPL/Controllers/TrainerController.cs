using GymManagementBLL.Services.Classes;
using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    [Authorize(Roles = "SuperAdmin")]
    public class TrainerController : Controller
    {
        private readonly ITrainerService _trainerService;

        public TrainerController(ITrainerService trainerService)
        {
            _trainerService = trainerService;
        }
        public IActionResult Index()
        {
            var trainer = _trainerService.GetAllTrainers();
            return View(trainer);
        }

        public IActionResult TrainerDetails(int id)
        {
            var trainer = _trainerService.GetTrainerDetails(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Member Not Found!";
                return RedirectToAction(nameof(Index));
            }
            return View(trainer);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult CreateTrainer(CreatedTrainerVM input)
        {
            if (!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing Data");
                return View(nameof(Create), input);
            }

            bool result = _trainerService.CreateTrainer(input);

            if (result)
                TempData["SuccessMessage"] = "Trainer Created Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Create Trainer, Phone Number or Email Already exists";

            return RedirectToAction(nameof(Index));

        }

        public IActionResult TrainerEdit(int id)
        {
            var trainer = _trainerService.GetTrainerForUpdate(id);

            if (trainer is null)
            {
                TempData["ErrorMessage"] = "Trainer Not Found!";
                return RedirectToAction(nameof(Index));
            }

            return View(trainer);
        }

        [HttpPost]
        public IActionResult TrainerEdit([FromRoute] int id, UpdatedTrainerVM input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            bool result = _trainerService.UpdateTrainer(id, input);

            if (result)
                TempData["SuccessMessage"] = "Trainer Updated Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Update Trainer";

            return RedirectToAction(nameof(Index));

        }

        //public IActionResult TrainerDelete([FromRoute] int id)
        //{
        //    if (id <= 0)
        //    {
        //        TempData["ErrorMessage"] = "Invalid Trainer Id!";
        //        return RedirectToAction(nameof(Index));
        //    }

        //    var trainer = _trainerService.GetTrainerDetails(id);
        //    if (trainer is null)
        //    {
        //        TempData["ErrorMessage"] = "Trainer Not Found!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}

        [HttpPost]
        public IActionResult DeleteConfirmed([FromForm] int id)
        {
            bool result = _trainerService.RemoveTrainer(id);

            if (result)
                TempData["SuccessMessage"] = "Trainer Deleted Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Delete Trainer";

            return RedirectToAction(nameof(Index));
        }
    }
}
