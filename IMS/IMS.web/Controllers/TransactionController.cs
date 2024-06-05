using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.Models.ViewModels;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DiaSymReader;
using System.Reflection.Metadata;

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
        private readonly ICrudService<StoreInfo> _storeInfo;
        private readonly IRawSqlRepository _rawSqlRepository;

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
            ICrudService<ProductInvoiceDetailInfo> productInvoiceDetailInfo,
             ICrudService<StoreInfo> storeInfo,
            IRawSqlRepository rawSqlRepository
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
            _storeInfo = storeInfo;
            _rawSqlRepository = rawSqlRepository;
        }
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var transactioinfo = await _productInvoiceInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            ViewBag.CustomerInfo = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);

            return View(transactioinfo);
        }

        public async Task<IActionResult> Transaction()
        {
            TransactionViewModel transactionViewModel = new TransactionViewModel();
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);

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

            return Json(new { batchList });
        }

        [HttpPost]
        [Route("/api/Transaction/getProductRate")]
        public async Task<IActionResult> GetProductRate(int ProductRateId)
        {
            var productDetail = await _productRateInfo.GetAsync(ProductRateId);

            return Json(new { productDetail });
        }

        [HttpPost]
        [Route("/api/Transaction/getCustomer")]
        public async Task<IActionResult> GetCustomer(int custometId)
        {
            var customerInfo = await _customerInfo.GetAsync(custometId);
            return Json(new { customerInfo });
        }

        [HttpPost]
        [Route("/api/Transaction/saveTransaction")]
        public async Task<int> SaveTransaction(TransactionViewModel transactionViewModel)
        {
            try
            {
                var userId = _userManager.GetUserId(HttpContext.User);
                var user = await _userManager.FindByIdAsync(userId);

                //var InvoiceNo = 0;
                //var productinvoiceinfo = await _productInvoiceInfo.GetAsync();
                transactionViewModel.ProductInvoiceInfo.TransactionDate = DateTime.Now;
                transactionViewModel.ProductInvoiceInfo.CreatedBy = userId;
                transactionViewModel.ProductInvoiceInfo.CreatedDate = DateTime.Now;
                transactionViewModel.ProductInvoiceInfo.StoreInfoId = user.StoreId;
                transactionViewModel.ProductInvoiceInfo.BillStatus = 1;
                transactionViewModel.ProductInvoiceInfo.IsActive = true;
                var invoiceId = await _productInvoiceInfo.InsertAsync(transactionViewModel.ProductInvoiceInfo);

                if (transactionViewModel.ProductInvoiceDetailInfos.Count() > 0)
                {

                    foreach (var items in transactionViewModel.ProductInvoiceDetailInfos)
                    {
                        ProductInvoiceDetailInfo productInvoiceDetailInfo = new ProductInvoiceDetailInfo();

                        productInvoiceDetailInfo.ProductInvoiceInfoId = invoiceId;
                        productInvoiceDetailInfo.ProductRateInfoId = items.ProductRateInfoId;
                        productInvoiceDetailInfo.Rate = items.Rate;
                        productInvoiceDetailInfo.Quantity = items.Quantity;
                        productInvoiceDetailInfo.Amount = items.Amount;
                        productInvoiceDetailInfo.CreatedBy = userId;
                        productInvoiceDetailInfo.CreatedDate = DateTime.Now;
                        await _productInvoiceDetailInfo.InsertAsync(productInvoiceDetailInfo);


                        var rateinfo = await _productRateInfo.GetAsync(items.ProductRateInfoId);
                        var product = await _productInfo.GetAsync(rateinfo.ProductInfoId);

                        TransactionInfo transactionInfo = new TransactionInfo();
                        transactionInfo.TransactionType = "Sell";
                        transactionInfo.CategoryInfoId = rateinfo.CategoryInfoId;
                        transactionInfo.ProductInfoId = rateinfo.ProductInfoId;
                        transactionInfo.UnitInfoId = product.UnitInfoId;
                        transactionInfo.ProductRateInfoId = items.ProductRateInfoId;
                        transactionInfo.Rate = items.Rate;
                        transactionInfo.Quantity = items.Quantity;
                        transactionInfo.Amount = items.Amount;
                        transactionInfo.IsActive = true;
                        transactionInfo.CreatedDate = DateTime.Now;
                        transactionInfo.CreatedBy = userId;
                        transactionInfo.StoreInfoId = user.StoreId;
                        await _transactionInfo.InsertAsync(transactionInfo);

                        rateinfo.SoldQuantity += items.Quantity;
                        rateinfo.RemainingQuantity -= items.Quantity;
                        rateinfo.ModifiedBy = userId;
                        rateinfo.ModifiedDate = DateTime.Now;
                        await _productRateInfo.UpdateAsync(rateinfo);

                        var stockdet = await _stockInfo.GetAsync(p => p.ProductInfoId == rateinfo.ProductInfoId && p.StoreInfoId == user.StoreId);
                        var qty = stockdet.Quantity - items.Quantity;
                        stockdet.Quantity = qty;
                        stockdet.ModifiedBy = userId;
                        stockdet.ModifiedDate = DateTime.Now;
                        await _stockInfo.UpdateAsync(stockdet);


                    }
                }
                return invoiceId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IActionResult> PrintReport(int Id)
        {

            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync();
            ViewBag.ProductRateInfos = await _productRateInfo.GetAllAsync();
            ViewBag.ProductInfos = await _productInfo.GetAllAsync();
            ViewBag.UnitInfos = await _unitInfo.GetAllAsync(p => p.IsActive == true);
            ViewBag.CustomerInfos = await _customerInfo.GetAllAsync();




            var invoiceInfo = await _productInvoiceInfo.GetAsync(Id);
            var user = await _userManager.FindByIdAsync(invoiceInfo.CreatedBy);
            ViewBag.TaxCreator = user.FirstName + ' ' + user.MiddleName + ' ' + user.LastName;
            TransactionViewModel transactionViewModel = new TransactionViewModel();
            transactionViewModel.ProductInvoiceInfo = invoiceInfo;
            decimal totalAmount = (decimal)invoiceInfo.TotalAmount; // Assuming TotalAmount is of a decimal type
            ViewBag.AmountInWord = NumberToWordsHelper.ConvertToWords(totalAmount);

            transactionViewModel.StoreInfo = await _storeInfo.GetAsync(user.StoreId);
            transactionViewModel.ProductInvoiceDetailInfos = await _productInvoiceDetailInfo.GetAllAsync(p => p.ProductInvoiceInfoId == Id);
            return View(transactionViewModel);
        }

        public static class NumberToWordsHelper
        {
            private static readonly string[] Units = { "Zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };
            private static readonly string[] Tens = { "", "", "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            public static string ConvertToWords(decimal number)
            {
                if (number == 0)
                    return "Zero Rupees";

                if (number < 0)
                    return "Minus " + ConvertToWords(Math.Abs(number));

                var words = "";

                int intPart = (int)number;
                int fractionalPart = (int)((number - intPart) * 100);

                words = ConvertToWords(intPart) + " Rupees";

                if (fractionalPart > 0)
                {
                    words += " and " + ConvertToWords(fractionalPart) + " Paise";
                }

                return words.Trim();
            }

            private static string ConvertToWords(int number)
            {
                if (number == 0)
                    return Units[0];

                var words = "";

                if ((number / 10000000) > 0)
                {
                    words += ConvertToWords(number / 10000000) + " Crore ";
                    number %= 10000000;
                }

                if ((number / 100000) > 0)
                {
                    words += ConvertToWords(number / 100000) + " Lakh ";
                    number %= 100000;
                }

                if ((number / 1000) > 0)
                {
                    words += ConvertToWords(number / 1000) + " Thousand ";
                    number %= 1000;
                }

                if ((number / 100) > 0)
                {
                    words += ConvertToWords(number / 100) + " Hundred ";
                    number %= 100;
                }

                if (number > 0)
                {
                    if (words != "")
                        words += "and ";

                    if (number < 20)
                        words += Units[number];
                    else
                    {
                        words += Tens[number / 10];
                        if ((number % 10) > 0)
                            words += "-" + Units[number % 10];
                    }
                }

                return words.Trim();
            }
        }


        [HttpPost]
        [Route("/api/Transaction/cancellTransaction")]
        public async Task<IActionResult> CancellTransaction(int invoiceId, string cancellationRemarks)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var productinvoiceinfo = await _productInvoiceInfo.GetAsync(invoiceId);
            productinvoiceinfo.BillStatus = 2;
            productinvoiceinfo.CancellationRemarks = cancellationRemarks;
            productinvoiceinfo.ModifiedBy = userId;
            productinvoiceinfo.ModifiedDate = DateTime.Now;
            await _productInvoiceInfo.UpdateAsync(productinvoiceinfo);
            return Json(1);
        }



    }
}
