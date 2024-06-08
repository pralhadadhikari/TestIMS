using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.Models.ViewModels;
using IMS.web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace IMS.web.Controllers
{
    [Authorize(Roles = "ADMIN,COUNTER,STORE")]
    public class DashboardController : Controller
    {
        private readonly ICrudService<SupplierInfo> _supplierInfo;
        private readonly ICrudService<CustomerInfo> _customerInfo;
        private readonly ICrudService<CategoryInfo> _categoryInfo;
        private readonly ICrudService<ProductInfo> _productInfo;
        private readonly IRawSqlRepository _rawSqlRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICrudService<StockInfo> _stockInfo;

        public DashboardController(ICrudService<SupplierInfo> supplierInfo,
           ICrudService<CustomerInfo> customerInfo,
           ICrudService<CategoryInfo> categoryInfo,
           ICrudService<ProductInfo> productInfo,
           IRawSqlRepository rawSqlRepository,
           UserManager<ApplicationUser> userManager,
           ICrudService<StockInfo> stockInfo)


        {
            _supplierInfo = supplierInfo;
            _customerInfo = customerInfo;
            _categoryInfo = categoryInfo;
            _productInfo = productInfo;
            _rawSqlRepository = rawSqlRepository;
            _userManager = userManager;
            _stockInfo = stockInfo;
        }
        public async Task<IActionResult> Index()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);

            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p=>p.StoreInfoId == user.StoreId);

            dashboardViewModel.StockInfos = await _stockInfo.GetAllAsync(p=>p.StoreInfoId== user.StoreId && p.Quantity<=10);

            var result = _rawSqlRepository.FromSql<DashboardList>(
               "usp_getDashboardList @storeId",
               new SqlParameter("@storeId", user.StoreId)
           ).ToList();

            dashboardViewModel.DashboardListInfos = result;


            var results = _rawSqlRepository.FromSql<DashboardIndex>(
              "usp_getDashboardIndex @storeId",
              new SqlParameter("@storeId", user.StoreId)
          ).ToList();
            dashboardViewModel.DashboardIndex = results.FirstOrDefault() ?? new DashboardIndex();
            return View(dashboardViewModel);
        }
    }
}
