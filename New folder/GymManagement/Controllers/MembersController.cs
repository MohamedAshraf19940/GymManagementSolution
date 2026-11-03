using GymManagement.BLL.Services.Attachment;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GymManagement.PL.Controllers
{
    public class MembersController : Controller
    {
        private readonly IMemberService _memberService;
        private readonly IAttachmentService _attachmentService;

        public MembersController(IMemberService memberService, IAttachmentService attachmentService)
        {
            _memberService = memberService;
            _attachmentService = attachmentService;
        }



        #region Picture : Get Member Picture

        [HttpGet]
        public async Task<IActionResult> Picture(int id)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id);

            if (member is null || string.IsNullOrEmpty(member.Photo)) return NotFound();


            var result = _attachmentService.GetFile(member.Photo, "MembersPhotos");
            return File(result.Value.stream, result.Value.contentType);

        }

        #endregion

        public async Task<IActionResult> Index()
        {
            var members = await _memberService.GetAllMembersAsync();


            return View(members);
        }

        //Get Member Details



        #region Create

        [HttpGet]
        public IActionResult Create() => View();
        [HttpPost]
        public async Task<IActionResult> CreateMember(CreateMemberViewModel member)
        {
            if (!ModelState.IsValid) return View(nameof(Create), member);

            var result = await _memberService.CreateMemberAsync(member);


            if (result)
            {
                TempData["SuccessMessage"] = "Member Created Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed To  Create Member  ";
            }

            return RedirectToAction(nameof(Index));



        }
        #endregion


        public async Task<IActionResult> MemberDetails(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }


        public async Task<IActionResult> HealthRecordDetails(int id, CancellationToken ct)
        {
            var result = await _memberService.GetHealthRecordAsync(id, ct);
            if (result is null)
            {
                TempData["ErrorMessage"] = "HealthRecord not found.";
                return RedirectToAction(nameof(Index));
            }
            return View(result);

        }



        #region Update

        [HttpGet]
        public async Task<IActionResult> EditMember(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberToUpdateAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member Not Found";
                return RedirectToAction(nameof(Index));
            }
            return View(member);
        }
        [HttpPost]
        public async Task<IActionResult> EditMember([FromRoute] int id, MemberToUpdateViewModel member, CancellationToken ct)
        {
            if (!ModelState.IsValid) return View(member);

            var result = await _memberService.UpdateMemberDetailsAsync(id, member, ct);


            if (result)
            {
                TempData["SuccessMessage"] = "Member Updated Successfully";
            }
            else
            {
                TempData["ErrorMessage"] = $"Failed To  Update Member  ";
            }

            return RedirectToAction(nameof(Index));



        }
        #endregion


        #region Delete
        [HttpGet]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            var member = await _memberService.GetMemberDetailsByIdAsync(id, ct);
            if (member is null)
            {
                TempData["ErrorMessage"] = "Member not found.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed([FromRoute] int id, CancellationToken ct)
        {
            var result = await _memberService.RemoveMemberAsync(id, ct);
            if (result)
            {
                TempData["SuccessMessage"] = "Member Deleted Successfully   ";
            }
            else
            {
                TempData["ErrorMessage"] = "Failed To Delete Member  ";
            }
            return RedirectToAction(nameof(Index));

        }
        #endregion

    }
}
