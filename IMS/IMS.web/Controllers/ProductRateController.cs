using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    public class ProductRateController : Controller
    {
        private readonly ICrudService<ProductInfo> _productInfo;
        private readonly ICrudService<CategoryInfo> _categoryInfo;
        private readonly ICrudService<UnitInfo> _unitInfo;
        private readonly ICrudService<StoreInfo> _storeInfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICrudService<ProductRateInfo> _productRateInfo;
        private readonly ICrudService<RackInfo> _rackInfo;
        private readonly ICrudService<StockInfo> _stockInfo;
        private readonly ICrudService<TransactionInfo> _transactionInfo;
        private readonly ICrudService<SupplierInfo> _supplierInfo;

        public ProductRateController(ICrudService<ProductInfo> productInfo,
            ICrudService<CategoryInfo> categoryInfo,
            ICrudService<UnitInfo> unitInfo,
            ICrudService<StoreInfo> storeInfo,
            UserManager<ApplicationUser> userManager,
            ICrudService<ProductRateInfo> productRateInfo,
            ICrudService<RackInfo> rackInfo,
            ICrudService<StockInfo> stockInfo,
            ICrudService<TransactionInfo> transactionInfo,
            ICrudService<SupplierInfo> supplierInfo)
        {
            _productInfo = productInfo;
            _categoryInfo = categoryInfo;
            _unitInfo = unitInfo;
            _storeInfo = storeInfo;
            _userManager = userManager;
            _productRateInfo = productRateInfo;
            _rackInfo = rackInfo;
            _stockInfo = stockInfo;
            _transactionInfo = transactionInfo;
            _supplierInfo = supplierInfo;
        }



        public async Task<IActionResult> Index()
        {
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true);           
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.RackInfos = await _rackInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.SupplierInfos = await _supplierInfo.GetAllAsync(p => p.IsActive == true);
            var rateRateInfo = await _productRateInfo.GetAllAsync();

            return View(rateRateInfo);
        }

        public async Task<IActionResult> AddEdit(int Id)
        {
            ProductRateInfo productRateInfo = new ProductRateInfo();

            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.RackInfos = await _rackInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.SupplierInfos = await _supplierInfo.GetAllAsync(p => p.IsActive == true);
            productRateInfo.PurchasedDate = DateTime.Now;
            productRateInfo.IsActive = true;

            if (Id > 0)
            {
                productRateInfo = await _productRateInfo.GetAsync(Id);
            }
            return View(productRateInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductRateInfo productRateInfo)
        {


            return RedirectToAction(nameof(Index));
        }


        [HttpPost]
        [Route("/api/ProductRate/getproduct")]
        public async Task<IActionResult> GetProduct(int CategoryId)
        {
            var productList=await _productInfo.GetAllAsync(p=>p.CategoryInfoId==CategoryId);

            return Json (new { productList });
        }

    }
   
}


