using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.web.Controllers;
using IMS.web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Identity.Client;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IMSUntTest.Controllers
{
    public class CustomerControllerTests
    {
        private readonly Mock<ICrudService<CustomerInfo>> _customerServiceMock;
        private readonly Mock<ICrudService<StoreInfo>> _storeServiceMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IRedisService> _redisServiceMock;
        public readonly CustomerController _controller;

        public CustomerControllerTests()
        {
            _customerServiceMock = new Mock<ICrudService<CustomerInfo>>();
            _storeServiceMock = new Mock<ICrudService<StoreInfo>>();
            _redisServiceMock = new Mock<IRedisService>();
            var userStore = new Mock<IUserStore<ApplicationUser>>();
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
               userStore.Object,
               null,
               null,
               null,
               null,
               null,
               null,
               null,
               null);
            _controller = new CustomerController(
                _customerServiceMock.Object,
                _storeServiceMock.Object,
                _userManagerMock.Object,
                 _redisServiceMock.Object);
            var httpContext = new DefaultHttpContext();

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            _controller.TempData = new TempDataDictionary(
                httpContext,
                Mock.Of<ITempDataProvider>());
        }
        private void SetupLoggedInUser()
        {
            var userId = "user-1";

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var identity = new ClaimsIdentity(
                claims,
                "TestAuthentication");

            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext.HttpContext.User = principal;

            var user = new ApplicationUser
            {
                Id = userId,
                StoreId = 10
            };

            _userManagerMock
                .Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);
        }

        // ============================================================
        // GET: Index
        // ============================================================

        [Fact]
        public async Task Index_ReturnsCustomersForCurrentStore()
        {
            // Arrange
            SetupLoggedInUser();

            var customers = new List<CustomerInfo>
            {
                new CustomerInfo { Id = 1, CustomerName = "Ram", StoreInfoId = 10 },
                new CustomerInfo { Id = 2, CustomerName = "Shyam", StoreInfoId = 10 }
            };

            _customerServiceMock
                .Setup(x => x.GetAllAsync(It.IsAny<System.Linq.Expressions.Expression<Func<CustomerInfo, bool>>>()))
                .ReturnsAsync(customers);

            // Act
            var result = await _controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsAssignableFrom<IEnumerable<CustomerInfo>>(viewResult.Model);

            Assert.Equal(2, model.Count());

            Assert.Contains(model, x => x.CustomerName == "Ram");
            Assert.Contains(model, x => x.CustomerName == "Shyam");
        }

        // ============================================================
        // GET: AddEdit
        // ============================================================

        [Fact]
        public async Task AddEdit_NewCustomer_ReturnsEmptyCustomer()
        {
            // Act
            var result = await _controller.AddEdit(0);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsType<CustomerInfo>(viewResult.Model);

            Assert.Equal(0, model.Id);
        }

        [Fact]
        public async Task AddEdit_ExistingCustomer_ReturnsCustomer()
        {
            // Arrange
            var customer = new CustomerInfo
            {
                Id = 1,
                CustomerName = "Ram",
                Email = "ram@test.com"
            };

            _customerServiceMock
                .Setup(x => x.GetAsync(1))
                .ReturnsAsync(customer);

            // Act
            var result = await _controller.AddEdit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsType<CustomerInfo>(viewResult.Model);

            Assert.Equal(1, model.Id);
            Assert.Equal("Ram", model.CustomerName);
            Assert.Equal("ram@test.com", model.Email);
        }

        // ============================================================
        // POST: AddEdit CREATE
        // ============================================================

        [Fact]
        public async Task AddEdit_NewCustomer_InsertsCustomer()
        {
            // Arrange
            SetupLoggedInUser();

            var customer = new CustomerInfo
            {
                Id = 0,
                CustomerName = "Ram",
                Email = "ram@test.com",
                PhoneNumber = "9800000000",
                Address = "Kathmandu",
                PanNo = "123456789"
            };

            // Act
            var result = await _controller.AddEdit(customer);

            // Assert
            var redirectResult =
                Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);

            _customerServiceMock.Verify(
                x => x.InsertAsync(
                    It.Is<CustomerInfo>(c =>
                        c.CustomerName == "Ram" &&
                        c.StoreInfoId == 10 &&
                        c.CreatedBy == "user-1"
                    )),
                Times.Once);

            Assert.Equal(
                "Data Added Sucessfully",
                _controller.TempData["success"]);
        }
    }
}
