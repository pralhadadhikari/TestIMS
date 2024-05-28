using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    //[Authorize(Policy ="ADMIN")]
    [Authorize(Roles = "SUPERADMIN")]
    public class StoreInfoController : Controller
    {
        private readonly ICrudService<StoreInfo> _storeCrudService;
        private readonly UserManager<ApplicationUser> _userManager;

        public StoreInfoController(ICrudService<StoreInfo> storeCrudService,
            UserManager<ApplicationUser> userManager)
        {
            _storeCrudService = storeCrudService;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var storeInfoList = await _storeCrudService.GetAllAsync();
            return View(storeInfoList);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            StoreInfo storeInfo = new StoreInfo();
            storeInfo.IsActive = true;
            if (id > 0)
            {
                storeInfo = await _storeCrudService.GetAsync(id);
            }

            return View(storeInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(StoreInfo storeInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    if (storeInfo.Id == 0)
                    {
                        storeInfo.CreatedDate = DateTime.Now;
                        storeInfo.CreatedBy = userId;
                        await _storeCrudService.InsertAsync(storeInfo);

                        TempData["success"] = "Data Added Sucessfully";
                    }
                    else
                    {
                        var OrgStoreInfo = await _storeCrudService.GetAsync(storeInfo.Id);
                        OrgStoreInfo.StoreName = storeInfo.StoreName;
                        OrgStoreInfo.Address = storeInfo.Address;
                        OrgStoreInfo.PhoneNumber = storeInfo.PhoneNumber;
                        OrgStoreInfo.PanNo = storeInfo.PanNo;
                        OrgStoreInfo.RegistrationNo = storeInfo.RegistrationNo;
                        OrgStoreInfo.IsActive = storeInfo.IsActive;
                        OrgStoreInfo.ModifiedDate = DateTime.Now;
                        OrgStoreInfo.ModifiedBy = userId;
                        await _storeCrudService.UpdateAsync(OrgStoreInfo);
                        TempData["success"] = "Data Updated Sucessfully";
                    }
                    return RedirectToAction(nameof(Index));
                }
                catch(Exception)
                {
                    TempData["error"] = "Something went wrong, please try again later";
                    return RedirectToAction(nameof(AddEdit));
                }
            }
            
            TempData["error"] = "Please input Valid Data";
            return RedirectToAction(nameof(AddEdit));
        }
                

        

        public async Task<IActionResult> Delete(int id)
        {
            var storeInfo= await _storeCrudService.GetAsync(id);
            _storeCrudService.Delete(storeInfo);
            TempData["error"] = "Data Deleted Sucessfully";
            return RedirectToAction("Index");
        }
    }
}
