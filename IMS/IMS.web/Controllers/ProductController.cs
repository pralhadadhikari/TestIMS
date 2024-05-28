using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ICrudService<ProductInfo> _productInfo;
        private readonly ICrudService<CategoryInfo> _categoryInfo;
        private readonly ICrudService<UnitInfo> _unitInfo;
        private readonly ICrudService<StoreInfo> _storeInfo;
        private readonly UserManager<ApplicationUser> _userManager;
        public ProductController(ICrudService<ProductInfo> productInfo,
            ICrudService<CategoryInfo> categoryInfo,
            ICrudService<UnitInfo> unitInfo,
            ICrudService<StoreInfo> storeInfo,
            UserManager<ApplicationUser> userManager
            )
        {
            _productInfo = productInfo;
            _categoryInfo = categoryInfo;
            _unitInfo = unitInfo;
            _storeInfo = storeInfo;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.CategoryInfos=await _categoryInfo.GetAllAsync(p=>p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);

            var productInfos = await _productInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            return View(productInfos);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ProductInfo productInfo = new ProductInfo();
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            productInfo.IsActive = true;
            if (id > 0)
            {
                productInfo = await _productInfo.GetAsync(id);
            }
            return View(productInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductInfo productInfo)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            if (ModelState.IsValid)
            {
                try
                {
                  


                    if (productInfo.ImageFile != null)
                    {
                        string fileDirectory = $"wwwroot/ProductImage";

                        if (!Directory.Exists(fileDirectory))
                        {
                            Directory.CreateDirectory(fileDirectory);
                        }
                        string uniqueFileName = Guid.NewGuid() + "_" + productInfo.ImageFile.FileName;
                        string filePath = Path.Combine(Path.GetFullPath($"wwwroot/ProductImage"), uniqueFileName);

                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await productInfo.ImageFile.CopyToAsync(fileStream);
                            productInfo.ImageUrl = $"ProductImage/" + uniqueFileName;

                        }

                    }

                    if (productInfo.Id == 0)
                    {
                        productInfo.CreatedDate = DateTime.Now;
                        productInfo.CreatedBy = userId;
                        productInfo.StoreInfoId = user.StoreId;
                        await _productInfo.InsertAsync(productInfo);

                        TempData["success"] = "Data Added Sucessfully";
                    }
                    else
                    {
                        var OrgproductInfo = await _productInfo.GetAsync(productInfo.Id);
                        OrgproductInfo.CategoryInfoId = productInfo.CategoryInfoId;
                        OrgproductInfo.ProductName = productInfo.ProductName;
                        OrgproductInfo.ProductDescription = productInfo.ProductDescription;
                        OrgproductInfo.UnitInfoId = productInfo.UnitInfoId;
                        if (productInfo.ImageFile != null)
                        {
                            OrgproductInfo.ImageUrl = productInfo.ImageUrl;
                        }
                        await _productInfo.UpdateAsync(OrgproductInfo);
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
