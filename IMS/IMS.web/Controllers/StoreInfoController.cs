using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    [Authorize]
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
            if(id > 0)
            {
                storeInfo = await _storeCrudService.GetAsync(id);
            }

            return View(storeInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(StoreInfo storeInfo)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            if (storeInfo.Id == 0)
            {
                storeInfo.CreatedDate = DateTime.Now;
                storeInfo.CreatedBy = userId;
                await _storeCrudService.InsertAsync(storeInfo);
            }
            else
            {
                var OrgStoreInfo = await _storeCrudService.GetAsync(storeInfo.Id);
                OrgStoreInfo.StoreName=storeInfo.StoreName;
                OrgStoreInfo.Address = storeInfo.Address;
                OrgStoreInfo.PhoneNumber=storeInfo.PhoneNumber;
                OrgStoreInfo.PanNo = storeInfo.PanNo;
                OrgStoreInfo.RegistrationNo = storeInfo.RegistrationNo;
                OrgStoreInfo.IsActive=storeInfo.IsActive;
                OrgStoreInfo.ModifiedDate = DateTime.Now;
                OrgStoreInfo.ModifiedBy = userId;
                await _storeCrudService.UpdateAsync(OrgStoreInfo);
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var storeInfo= await _storeCrudService.GetAsync(id);
            _storeCrudService.Delete(storeInfo);
            return RedirectToAction("Index");
        }
    }
}
