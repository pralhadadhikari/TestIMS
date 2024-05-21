using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    public class RackController : Controller
    {
        private readonly ICrudService<RackInfo> _rackInfo;
        private readonly ICrudService<StoreInfo> _storeInfo;
        private readonly UserManager<ApplicationUser> _userManager;

        public RackController(ICrudService<RackInfo> rackInfo,
            ICrudService<StoreInfo> storeInfo,
            UserManager<ApplicationUser> userManager)
        {
            _rackInfo = rackInfo;
            _storeInfo = storeInfo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var rackInfos = await _rackInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            return View(rackInfos);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            RackInfo rackInfo = new RackInfo();
            rackInfo.IsActive = true;
            if (id > 0)
            {
                rackInfo = await _rackInfo.GetAsync(id);
            }

            return View(rackInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(RackInfo rackInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);
                    if (rackInfo.Id == 0)
                    {
                        rackInfo.CreatedDate = DateTime.Now;
                        rackInfo.CreatedBy = userId;
                        rackInfo.StoreInfoId = user.StoreId;
                        await _rackInfo.InsertAsync(rackInfo);

                        TempData["success"] = "Data Added Sucessfully";
                    }
                    else
                    {
                        var OrgrackInfo = await _rackInfo.GetAsync(rackInfo.Id);
                        OrgrackInfo.RackName = rackInfo.RackName;                        ;
                        OrgrackInfo.IsActive = rackInfo.IsActive;
                        OrgrackInfo.ModifiedDate = DateTime.Now;
                        OrgrackInfo.ModifiedBy = userId;
                        await _rackInfo.UpdateAsync(OrgrackInfo);
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
