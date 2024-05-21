using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    public class SupplierController : Controller
    {
        private readonly ICrudService<SupplierInfo> _supplierInfo;
        private readonly ICrudService<StoreInfo> _storeInfo;
        private readonly UserManager<ApplicationUser> _userManager;

        public SupplierController(ICrudService<SupplierInfo> supplierInfo,
            ICrudService<StoreInfo> storeInfo,
            UserManager<ApplicationUser> userManager
            )
        {
            _supplierInfo = supplierInfo;
            _storeInfo = storeInfo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var supplierInfos = await _supplierInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            return View(supplierInfos);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            SupplierInfo supplierInfo = new SupplierInfo();
            supplierInfo.IsActive = true;
            if (id > 0)
            {
                supplierInfo = await _supplierInfo.GetAsync(id);
            }

            return View(supplierInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(SupplierInfo supplierInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);
                    if (supplierInfo.Id == 0)
                    {
                        supplierInfo.CreatedDate = DateTime.Now;
                        supplierInfo.CreatedBy = userId;
                        supplierInfo.StoreInfoId = user.StoreId;
                        await _supplierInfo.InsertAsync(supplierInfo);

                        TempData["success"] = "Data Added Sucessfully";
                    }
                    else
                    {
                        var OrgsupplierInfo = await _supplierInfo.GetAsync(supplierInfo.Id);
                        OrgsupplierInfo.SupplierName = supplierInfo.SupplierName;
                        OrgsupplierInfo.ContactPerson = supplierInfo.ContactPerson;
                        OrgsupplierInfo.SupplierName = supplierInfo.SupplierName;
                        OrgsupplierInfo.PhoneNumber = supplierInfo.PhoneNumber;
                        OrgsupplierInfo.Address = supplierInfo.Address;
                        OrgsupplierInfo.Email = supplierInfo.Email;
                        OrgsupplierInfo.IsActive = supplierInfo.IsActive;
                        OrgsupplierInfo.ModifiedDate = DateTime.Now;
                        OrgsupplierInfo.ModifiedBy = userId;
                        await _supplierInfo.UpdateAsync(OrgsupplierInfo);
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
