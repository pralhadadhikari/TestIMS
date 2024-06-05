﻿using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.Models.ViewModels;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace IMS.web.Controllers
{
    public class ReportController : Controller
    {
        private readonly ICrudService<SupplierInfo> _supplierInfo;
        private readonly ICrudService<CustomerInfo> _customerInfo;
        private readonly ICrudService<CategoryInfo> _categoryInfo;
        private readonly ICrudService<ProductInfo> _productInfo;
        private readonly IRawSqlRepository _rawSqlRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public ReportController(ICrudService<SupplierInfo> supplierInfo,
           ICrudService<CustomerInfo> customerInfo,
           ICrudService<CategoryInfo> categoryInfo,
           ICrudService<ProductInfo> productInfo,
           IRawSqlRepository rawSqlRepository,
           UserManager<ApplicationUser> userManager)
        {
            _supplierInfo = supplierInfo;
            _customerInfo = customerInfo;
            _categoryInfo = categoryInfo;
            _productInfo = productInfo;
            _rawSqlRepository = rawSqlRepository;
            _userManager = userManager;
        }
        public async Task <IActionResult> Index(ReportViewModel reportViewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.SupplierInfo = await _supplierInfo.GetAllAsync(p=>p.StoreInfoId==user.StoreId);
            ViewBag.CustomerInfo = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
           
            reportViewModel.SearchCriteria = new SearchCriteria();

            return View(reportViewModel);
        }

        public async Task<IActionResult> Search(ReportViewModel reportViewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.SupplierInfo = await _supplierInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            ViewBag.CustomerInfo = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            var customerId = reportViewModel.SearchCriteria.CustomerId.HasValue
                    ? new SqlParameter("@customerId", SqlDbType.Int) { Value = reportViewModel.SearchCriteria.CustomerId.Value }
                    : new SqlParameter("@customerId", SqlDbType.Int) { Value = DBNull.Value };

            var paymentMethodId = reportViewModel.SearchCriteria.PaymentMethod.HasValue
                            ? new SqlParameter("@PaymentMethodId", SqlDbType.Int) { Value = reportViewModel.SearchCriteria.PaymentMethod.Value }
                            : new SqlParameter("@PaymentMethodId", SqlDbType.Int) { Value = DBNull.Value };

            var startDate = reportViewModel.SearchCriteria.StartDate.HasValue
                            ? new SqlParameter("@startDate", SqlDbType.DateTime) { Value = reportViewModel.SearchCriteria.StartDate.Value }
                            : new SqlParameter("@startDate", SqlDbType.DateTime) { Value = DBNull.Value };

            var endDate = reportViewModel.SearchCriteria.EndDate.HasValue
                            ? new SqlParameter("@enddate", SqlDbType.DateTime) { Value = reportViewModel.SearchCriteria.EndDate.Value }
                            : new SqlParameter("@enddate", SqlDbType.DateTime) { Value = DBNull.Value };

            var result = _rawSqlRepository.FromSql<CustomReportViewModel>(
                "usp_GetTransactionInfo @customerId, @PaymentMethodId, @startDate, @enddate",
                customerId,
                paymentMethodId,
                startDate,
                endDate
            ).ToList();
            reportViewModel.CustomReportViewModels = result;


            return View(nameof(Index), reportViewModel);
        }




        public async Task<IActionResult> DetailReport(ReportViewModel reportViewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.SupplierInfo = await _supplierInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            ViewBag.CustomerInfo = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);

            reportViewModel.SearchCriteria = new SearchCriteria();

            return View(reportViewModel);
        }

        public async Task<IActionResult> SearchDetail(ReportViewModel reportViewModel)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            ViewBag.SupplierInfo = await _supplierInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            ViewBag.CustomerInfo = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            ViewBag.CategoryInfos = await _categoryInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);
            ViewBag.ProductInfos = await _productInfo.GetAllAsync(p => p.IsActive == true && p.StoreInfoId == user.StoreId);

            var customerId = reportViewModel.SearchCriteria.CustomerId.HasValue
                    ? new SqlParameter("@customerId", SqlDbType.Int) { Value = reportViewModel.SearchCriteria.CustomerId.Value }
                    : new SqlParameter("@customerId", SqlDbType.Int) { Value = DBNull.Value };
            var supplierId = reportViewModel.SearchCriteria.SupplierId.HasValue
                    ? new SqlParameter("@supplierId", SqlDbType.Int) { Value = reportViewModel.SearchCriteria.SupplierId.Value }
                    : new SqlParameter("@supplierId", SqlDbType.Int) { Value = DBNull.Value };
            var categoryId = reportViewModel.SearchCriteria.CategoryId.HasValue
                    ? new SqlParameter("@categoryId", SqlDbType.Int) { Value = reportViewModel.SearchCriteria.CategoryId.Value }
                    : new SqlParameter("@categoryId", SqlDbType.Int) { Value = DBNull.Value };
            var productId = reportViewModel.SearchCriteria.ProductId.HasValue
                    ? new SqlParameter("@productId", SqlDbType.Int) { Value = reportViewModel.SearchCriteria.ProductId.Value }
                    : new SqlParameter("@productId", SqlDbType.Int) { Value = DBNull.Value };

            var paymentMethodId = reportViewModel.SearchCriteria.PaymentMethod.HasValue
                            ? new SqlParameter("@PaymentMethodId", SqlDbType.Int) { Value = reportViewModel.SearchCriteria.PaymentMethod.Value }
                            : new SqlParameter("@PaymentMethodId", SqlDbType.Int) { Value = DBNull.Value };

            var startDate = reportViewModel.SearchCriteria.StartDate.HasValue
                            ? new SqlParameter("@startDate", SqlDbType.DateTime) { Value = reportViewModel.SearchCriteria.StartDate.Value }
                            : new SqlParameter("@startDate", SqlDbType.DateTime) { Value = DBNull.Value };

            var endDate = reportViewModel.SearchCriteria.EndDate.HasValue
                            ? new SqlParameter("@enddate", SqlDbType.DateTime) { Value = reportViewModel.SearchCriteria.EndDate.Value }
                            : new SqlParameter("@enddate", SqlDbType.DateTime) { Value = DBNull.Value };

            var result = _rawSqlRepository.FromSql<CustomReportViewModel>(
                "usp_GetDetailTransactionInfo @customerId, @supplierId, @categoryId, @productId,  @PaymentMethodId, @startDate, @enddate",
                customerId,
                supplierId,
                categoryId,
                productId,
                paymentMethodId,
                startDate,
                endDate
            ).ToList();
            reportViewModel.CustomReportViewModels = result;


            return View(nameof(Index), reportViewModel);
        }


    }
}
