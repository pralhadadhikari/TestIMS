using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.web.Data;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICrudService<CategoryInfo> _categoryInfo;
        private readonly ICrudService<StoreInfo> _storeInfo;
        private readonly UserManager<ApplicationUser> _userManager;

        public CategoryController(ICrudService<CategoryInfo> categoryInfo,
            ICrudService<StoreInfo> storeInfo,
            UserManager<ApplicationUser> userManager
            )
        {
            _categoryInfo = categoryInfo;
            _storeInfo = storeInfo;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {           
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);            
            var categoryinfo=await _categoryInfo.GetAllAsync(p=>p.StoreInfoId== user.StoreId);
            return View(categoryinfo);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            CategoryInfo categoryInfo = new CategoryInfo();
            categoryInfo.IsActive = true;
            if (id > 0)
            {
                categoryInfo = await _categoryInfo.GetAsync(id);
            }

            return View(categoryInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(CategoryInfo categoryInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);
                    if (categoryInfo.Id == 0)
                    {
                        categoryInfo.CreatedDate = DateTime.Now;
                        categoryInfo.CreatedBy = userId;
                        categoryInfo.StoreInfoId = user.StoreId;
                        await _categoryInfo.InsertAsync(categoryInfo);

                        TempData["success"] = "Data Added Sucessfully";
                    }
                    else
                    {
                        var OrgCategoryInfo = await _categoryInfo.GetAsync(categoryInfo.Id);
                        OrgCategoryInfo.CategoryName = categoryInfo.CategoryName;
                        OrgCategoryInfo.CategoryDescription = categoryInfo.CategoryDescription;                        
                        OrgCategoryInfo.IsActive = categoryInfo.IsActive;
                        OrgCategoryInfo.ModifiedDate = DateTime.Now;
                        OrgCategoryInfo.ModifiedBy = userId;
                        await _categoryInfo.UpdateAsync(OrgCategoryInfo);
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
