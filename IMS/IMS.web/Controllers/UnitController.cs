using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Runtime;

namespace IMS.web.Controllers
{
    public class UnitController : Controller
    {
        private readonly ICrudService<UnitInfo> _unitInfo;
        private readonly UserManager<ApplicationUser> _userManager;
        public UnitController(ICrudService<UnitInfo> unitInfo,
            UserManager<ApplicationUser> userManager)
        {
            _unitInfo = unitInfo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var unitInfoList = await _unitInfo.GetAllAsync();
            return View(unitInfoList);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            UnitInfo unitInfo = new UnitInfo();
            unitInfo.IsActive = true;
            if (id > 0)
            {
                unitInfo = await _unitInfo.GetAsync(id);
            }

            return View(unitInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(UnitInfo unitInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    if (unitInfo.Id == 0)
                    {
                        unitInfo.CreatedDate = DateTime.Now;
                        unitInfo.CreatedBy = userId;
                        await _unitInfo.InsertAsync(unitInfo);

                        TempData["success"] = "Data Added Sucessfully";
                    }
                    else
                    {
                        var OrgunitInfo = await _unitInfo.GetAsync(unitInfo.Id);
                        OrgunitInfo.UnitName = unitInfo.UnitName;                        
                        OrgunitInfo.IsActive = unitInfo.IsActive;
                        OrgunitInfo.ModifiedDate = DateTime.Now;
                        OrgunitInfo.ModifiedBy = userId;
                        await _unitInfo.UpdateAsync(OrgunitInfo);
                        TempData["success"] = "Data Updated Sucessfully";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception)
                {
                    TempData["error"] = "Something went wrong, please try again later";
                    return RedirectToAction(nameof(AddEdit));
                }
            }

            TempData["error"] = "Please input Valid Data";
            return RedirectToAction(nameof(AddEdit));
        }

    }
}
