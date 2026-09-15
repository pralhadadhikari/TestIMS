using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.web.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IMS.web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICrudService<CustomerInfo> _customerInfo;
        private readonly ICrudService<StoreInfo> _storeInfo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IRedisService _redisService;

        public CustomerController(ICrudService<CustomerInfo> customerInfo,
            ICrudService<StoreInfo> storeInfo,
            UserManager<ApplicationUser> userManager,
            IRedisService redisService
            )
        {
            _customerInfo = customerInfo;
            _storeInfo = storeInfo;
            _userManager = userManager;
            _redisService = redisService;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            var customerInfos = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            return View(customerInfos);
        }

        public async Task<IActionResult> AddEdit(int id)
        {
            CustomerInfo customerInfo = new CustomerInfo();           
            if (id > 0)
            {
                customerInfo = await _customerInfo.GetAsync(id);
            }

            return View(customerInfo);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(CustomerInfo customerInfo)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var userId = _userManager.GetUserId(HttpContext.User);
                    var user = await _userManager.FindByIdAsync(userId);
                    if (customerInfo.Id == 0)
                    {
                        customerInfo.CreatedDate = DateTime.Now;
                        customerInfo.CreatedBy = userId;
                        customerInfo.StoreInfoId = user.StoreId;
                        await _customerInfo.InsertAsync(customerInfo);
                        await _redisService.RemoveAsync($"ims:store:{customerInfo.StoreInfoId}:customers");

                        TempData["success"] = "Data Added Sucessfully";
                    }
                    else
                    {
                        var OrgcustomerInfo = await _customerInfo.GetAsync(customerInfo.Id);
                        OrgcustomerInfo.CustomerName = customerInfo.CustomerName;
                        OrgcustomerInfo.Email = customerInfo.Email;
                        OrgcustomerInfo.PhoneNumber = customerInfo.PhoneNumber;                        
                        OrgcustomerInfo.Address = customerInfo.Address;
                        OrgcustomerInfo.PanNo = customerInfo.PanNo;                        
                        await _customerInfo.UpdateAsync(OrgcustomerInfo);
                        TempData["success"] = "Data Updated Sucessfully";
                        await _redisService.RemoveAsync($"ims:customer:{customerInfo.Id}");
                        await _redisService.RemoveAsync($"ims:store:{customerInfo.StoreInfoId}:customers");
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

        [HttpPost]
        [Route("/api/Customer/addCustomer")]
        public async Task<IActionResult> AddCustomer(CustomerInfo model)
        {
            var userId = _userManager.GetUserId(HttpContext.User);
            var user = await _userManager.FindByIdAsync(userId);
            model.CreatedDate = DateTime.Now;
            model.CreatedBy = userId;
            model.StoreInfoId = user.StoreId;
            var result = await _customerInfo.InsertAsync(model);
            model.Id = result;

            await _redisService.RemoveAsync($"ims:store:{model.StoreInfoId}:customers");

            return Json (model);
        }
        public async Task<IActionResult> Test()
        {
            var userId = "testuser";
            var user = await _userManager.FindByIdAsync(userId);
            var customerInfos = await _customerInfo.GetAllAsync(p => p.StoreInfoId == user.StoreId);
            return View(customerInfos);
        }
    }
}
