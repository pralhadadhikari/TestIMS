using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using IMS.web.Controllers;
using IMS.web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace IMSUntTest.Controllers
{
    public class CategoryControllerTests
    {
        private readonly Mock<ICrudService<CategoryInfo>> _categoryServiceMock;
        private readonly Mock<ICrudService<StoreInfo>> _storeServiceMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly Mock<IRedisService> _redisServiceMock;

        private readonly CategoryController _controller;
        public CategoryControllerTests()
        {
            _categoryServiceMock = new Mock<ICrudService<CategoryInfo>>();
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

            _controller = new CategoryController(
                _categoryServiceMock.Object,
                _storeServiceMock.Object,
                _userManagerMock.Object,
                 _redisServiceMock.Object);

            var httpContext = new DefaultHttpContext();

            // Assign HttpContext to ControllerContext
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };

            // Setup TempData
            _controller.TempData = new TempDataDictionary(
                httpContext,
                Mock.Of<ITempDataProvider>());
        }
        [Fact]
        public async Task AddEdit_NewCategory_ReturnsViewWithActiveCategory()
        {
            // Act
            var result = await _controller.AddEdit(0);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsType<CategoryInfo>(viewResult.Model);

            Assert.Equal(0, model.Id);
            Assert.True(model.IsActive);
        }
        [Fact]
        public async Task AddEdit_ExistingCategory_ReturnsExistingCategory()
        {
            // Arrange
            var category = new CategoryInfo
            {
                Id = 1,
                CategoryName = "Medicine",
                CategoryDescription = "Medicine Category",
                IsActive = true
            };

            _categoryServiceMock
                .Setup(x => x.GetAsync(1))
                .ReturnsAsync(category);

            // Act
            var result = await _controller.AddEdit(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);

            var model = Assert.IsType<CategoryInfo>(viewResult.Model);

            Assert.Equal(1, model.Id);
            Assert.Equal("Medicine", model.CategoryName);
            Assert.Equal("Medicine Category", model.CategoryDescription);
            Assert.True(model.IsActive);

            // Verify GetAsync was called
            _categoryServiceMock.Verify(
                x => x.GetAsync(1),
                Times.Once);
        }
        [Fact]
        public async Task AddEdit_NewCategory_InsertsCategory()
        {
            // Arrange

            var userId = "user-1";

            var user = new ApplicationUser
            {
                Id = userId,
                StoreId = 10
            };

            // Create authenticated user
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId)
    };

            var identity = new ClaimsIdentity(
                claims,
                "TestAuthentication");

            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext.HttpContext.User = principal;

            // Mock GetUserId()
            _userManagerMock
                .Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            // Mock FindByIdAsync()
            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            // Category submitted from form
            var category = new CategoryInfo
            {
                Id = 0,
                CategoryName = "Medicine",
                CategoryDescription = "Medicine Category",
                IsActive = true
            };

            // Act
            var result = await _controller.AddEdit(category);

            // Assert

            var redirectResult =
                Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);

            // Verify InsertAsync was called
            _categoryServiceMock.Verify(
                x => x.InsertAsync(
                    It.Is<CategoryInfo>(c =>
                        c.Id == 0 &&
                        c.CategoryName == "Medicine" &&
                        c.CategoryDescription == "Medicine Category" &&
                        c.StoreInfoId == 10 &&
                        c.CreatedBy == userId &&
                        c.IsActive == true
                    )),
                Times.Once);

            // Verify success message
            Assert.Equal(
                "Data Added Sucessfully",
                _controller.TempData["success"]);
        }

        // ============================================================
        // POST: AddEdit - UPDATE
        // ============================================================

        [Fact]
        public async Task AddEdit_ExistingCategory_UpdatesCategory()
        {
            // Arrange

            var userId = "33a30e29-6f3c-4594-99e9-0b5b58cfb74b";

            var user = new ApplicationUser
            {
                Id = userId,
                StoreId = 1
            };

            // Create logged-in user
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, userId)
    };

            var identity = new ClaimsIdentity(
                claims,
                "TestAuthentication");

            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext.HttpContext.User = principal;

            // Mock GetUserId()
            _userManagerMock
                .Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            // Mock FindByIdAsync()
            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            // Existing category from database
            var existingCategory = new CategoryInfo
            {
                Id = 1,
                CategoryName = "Old Name",
                CategoryDescription = "Old Description",
                IsActive = true
            };

            _categoryServiceMock
                .Setup(x => x.GetAsync(1))
                .ReturnsAsync(existingCategory);

            // Data submitted from form
            var category = new CategoryInfo
            {
                Id = 1,
                CategoryName = "Updated Name",
                CategoryDescription = "Updated Description",
                IsActive = false
            };

            // Act
            var result = await _controller.AddEdit(category);

            // Assert

            var redirectResult =
                Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("Index", redirectResult.ActionName);

            // Verify UpdateAsync was called with updated values
            _categoryServiceMock.Verify(
                x => x.UpdateAsync(
                    It.Is<CategoryInfo>(c =>
                        c.Id == 1 &&
                        c.CategoryName == "Updated Name" &&
                        c.CategoryDescription == "Updated Description" &&
                        c.IsActive == false &&
                        c.ModifiedBy == userId
                    )),
                Times.Once);

            // Verify success message
            Assert.Equal(
                "Data Updated Sucessfully",
                _controller.TempData["success"]);
        }


        // ============================================================
        // POST: AddEdit - INVALID MODEL
        // ============================================================

        [Fact]
        public async Task AddEdit_InvalidModelState_ReturnsError()
        {
            // Arrange

            _controller.ModelState.AddModelError(
                "CategoryName",
                "Category Name is required");

            var category = new CategoryInfo
            {
                Id = 0
            };

            // Act
            var result = await _controller.AddEdit(category);

            // Assert

            var redirectResult =
                Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("AddEdit", redirectResult.ActionName);

            Assert.Equal(
                "Please input Valid Data",
                _controller.TempData["error"]);

            // Insert should NOT happen
            _categoryServiceMock.Verify(
                x => x.InsertAsync(It.IsAny<CategoryInfo>()),
                Times.Never);
        }


        // ============================================================
        // POST: AddEdit - EXCEPTION
        // ============================================================

        [Fact]
        public async Task AddEdit_WhenInsertThrowsException_ReturnsError()
        {
            // Arrange

            var userId = "user-1";

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

            // Force InsertAsync to throw exception
            _categoryServiceMock
                .Setup(x => x.InsertAsync(It.IsAny<CategoryInfo>()))
                .ThrowsAsync(new Exception("Database error"));

            var category = new CategoryInfo
            {
                Id = 0,
                CategoryName = "Medicine",
                CategoryDescription = "Medicine Category",
                IsActive = true
            };

            // Act
            var result = await _controller.AddEdit(category);

            // Assert

            var redirectResult =
                Assert.IsType<RedirectToActionResult>(result);

            Assert.Equal("AddEdit", redirectResult.ActionName);

            Assert.Equal(
                "Something went wrong, please try again later",
                _controller.TempData["error"]);
        }


        // ============================================================
        // GET: Index
        // ============================================================

        [Fact]
        public async Task Index_ReturnsCategoriesForCurrentUsersStore()
        {
            // Arrange

            var userId = "user-1";

            var user = new ApplicationUser
            {
                Id = userId,
                StoreId = 10
            };

            // Create authenticated user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            };

            var identity = new ClaimsIdentity(
                claims,
                "TestAuthentication");

            var principal = new ClaimsPrincipal(identity);

            _controller.ControllerContext.HttpContext.User = principal;

            // Mock UserManager
            _userManagerMock
                .Setup(x => x.GetUserId(It.IsAny<ClaimsPrincipal>()))
                .Returns(userId);

            _userManagerMock
                .Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            // Categories
            var categories = new List<CategoryInfo>
            {
                new CategoryInfo
                {
                    Id = 1,
                    CategoryName = "Medicine",
                    StoreInfoId = 10
                },
                new CategoryInfo
                {
                    Id = 2,
                    CategoryName = "Surgical",
                    StoreInfoId = 10
                }
            };

            _categoryServiceMock
                .Setup(x => x.GetAllAsync(
                    It.IsAny<System.Linq.Expressions.Expression<Func<CategoryInfo, bool>>>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _controller.Index();

            // Assert

            var viewResult =
                Assert.IsType<ViewResult>(result);

            var model =
                Assert.IsAssignableFrom<IEnumerable<CategoryInfo>>(
                    viewResult.Model);

            Assert.Equal(2, model.Count());

            Assert.Contains(
                model,
                x => x.CategoryName == "Medicine");

            Assert.Contains(
                model,
                x => x.CategoryName == "Surgical");

            _categoryServiceMock.Verify(
                 x => x.GetAllAsync(
                     It.IsAny<System.Linq.Expressions.Expression<Func<CategoryInfo, bool>>>()),
                 Times.Once);
        }
    }
}
