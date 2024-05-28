using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.Models.ViewModels;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    public class TransactionController : Controller
    {
        private readonly ICrudService<ProductInfo> _productInfo;
        private readonly ICrudService<CategoryInfo> _categoryInfo;
        private readonly ICrudService<UnitInfo> _unitInfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICrudService<ProductRateInfo> _productRateInfo;
        private readonly ICrudService<RackInfo> _rackInfo;
        private readonly ICrudService<StockInfo> _stockInfo;
        private readonly ICrudService<TransactionInfo> _transactionInfo;
        private readonly ICrudService<SupplierInfo> _supplierInfo;
        private readonly ICrudService<CustomerInfo> _customerInfo;
        private readonly ICrudService<ProductInvoiceInfo> _productInvoiceInfo;
        private readonly ICrudService<ProductInvoiceDetailInfo> _productInvoiceDetailInfo;

        public TransactionController(ICrudService<ProductInfo> productInfo,
            ICrudService<CategoryInfo> categoryInfo,
            ICrudService<UnitInfo> unitInfo,            
            UserManager<ApplicationUser> userManager,
            ICrudService<ProductRateInfo> productRateInfo,
            ICrudService<RackInfo> rackInfo,
            ICrudService<StockInfo> stockInfo,
            ICrudService<TransactionInfo> transactionInfo,
            ICrudService<SupplierInfo> supplierInfo,
            ICrudService<CustomerInfo> customerInfo,
            ICrudService<ProductInvoiceInfo> productInvoiceInfo,
            ICrudService<ProductInvoiceDetailInfo> productInvoiceDetailInfo
            )
        {
            _productInfo = productInfo;
            _categoryInfo = categoryInfo;
            _unitInfo = unitInfo;
            _userManager = userManager;
            _productRateInfo = productRateInfo;
            _rackInfo = rackInfo;
            _stockInfo = stockInfo;
            _transactionInfo = transactionInfo;
            _supplierInfo = supplierInfo;
            _customerInfo = customerInfo;
            _productInvoiceInfo = productInvoiceInfo;
            _productInvoiceDetailInfo = productInvoiceDetailInfo;
        }
        public async Task <IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var transactioinfo = await _productInvoiceInfo.GetAllAsync();
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);            
            return View(transactioinfo);
        }

        public async Task <IActionResult> Transaction()
        {
            TransactionViewModel transactionViewModel=new TransactionViewModel();
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true );

            ViewBag.RackInfos = await _rackInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.SupplierInfos = await _supplierInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.CustomerInfos = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            return View(transactionViewModel);
        }


        [HttpPost]
        [Route("/api/Transaction/getproduct")]
        public async Task<IActionResult> GetProduct(int CategoryId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var productList = await _productInfo.GetAllAsync(p => p.CategoryInfoId == CategoryId && p.StoreInfoId == user.StoreId);

            return Json(new { productList });
        }

        [HttpPost]
        [Route("/api/Transaction/getUnit")]
        public async Task<IActionResult> GetUnit(int ProductId)
        {
            var product = await _productInfo.GetAsync(ProductId);

            return Json(new { product });
        }



        [HttpPost]
        [Route("/api/Transaction/getBatch")]
        public async Task<IActionResult> GetBatch(int ProductId)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var batchList = await _productRateInfo.GetAllAsync(p => p.ProductInfoId == ProductId && p.StoreInfoId == user.StoreId);

            return Json(new { batchList});
        }

        [HttpPost]
        [Route("/api/Transaction/getProductRate")]
        public async Task<IActionResult> GetProductRate(int ProductRateId)
        {
            var productDetail = await _productRateInfo.GetAsync(ProductRateId);

            return Json(new { productDetail});
        }

    }
}
