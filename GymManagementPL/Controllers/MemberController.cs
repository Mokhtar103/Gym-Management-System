using GymManagementBLL.Services.Interfaces;
using GymManagementBLL.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GymManagementPL.Controllers
{
    public class MemberController : Controller
    {
        private readonly IMemberService _memberService;

        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public IActionResult Index()
        {
            var member = _memberService.GetAllMembers();
            return View(member);
        }

        public IActionResult MemberDetails(int id)
        {
            var member = _memberService.GetMemberDetails(id);

            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found!";
                return RedirectToAction(nameof(Index));
            }

            return View(member);
        }

        public IActionResult HealthRecordDetails(int id)
        {
            var memberHealthRecord = _memberService.GetMemberHealthDetails(id);

            if (memberHealthRecord is null)
            {
                TempData["ErrorMessage"] = "Member Not Found!";
                return RedirectToAction(nameof(Index));
            }
                

            return View(memberHealthRecord);

        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateMember(CreatedMemberVM input)
        {
            if(!ModelState.IsValid)
            {
                ModelState.AddModelError("DataMissed", "Check Missing Data");
                return View(nameof(Create), input);
            }

            bool result = _memberService.CreateMember(input);

            if (result)
                TempData["SuccessMessage"] = "Member Created Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Create Member, Phone Number or Email Already exists";

            return RedirectToAction(nameof(Index));

        }

        public IActionResult MemberEdit(int id)
        {
            var member = _memberService.GetMemberForUpdate(id);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found!";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }

        [HttpPost]
        public IActionResult MemberEdit([FromRoute] int id, UpdatedMemberVM input)
        {
            if (!ModelState.IsValid)
            {
                return View(input);
            }

            bool result = _memberService.UpdateMember(id, input);

            if (result)
                TempData["SuccessMessage"] = "Member Updated Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Update Member";

            return RedirectToAction(nameof(Index));

        }

        //public IActionResult MemberDelete([FromRoute] int id)
        //{
        //    if (id <= 0)
        //    {
        //        TempData["ErrorMessage"] = "Invalid Member Id!";
        //        return RedirectToAction(nameof(Index));
        //    } 

        //    var member = _memberService.GetMemberDetails(id);
        //    if (member is null)
        //    {
        //        TempData["ErrorMessage"] = "Member Not Found!";
        //        return RedirectToAction(nameof(Index));
        //    }
        //    return View();
        //}

        public IActionResult DeleteConfirmed([FromForm] int id)
        {
            bool result = _memberService.RemoveMember(id);

            if (result)
                TempData["SuccessMessage"] = "Member Deleted Successfully!";
            else
                TempData["ErrorMessage"] = "Failed to Delete Member";

            return RedirectToAction(nameof(Index));
        }
    }
}
