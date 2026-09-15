using System;
using System.Threading;
using System.Threading.Tasks;
using IIMS.Application.Common.Interface;
using IMS.Application.Features.Categories.Command.CreateCategory;
using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using Moq;
using Xunit;

namespace IMSUntTest.Features.Categories.Command.CreateCategory
{
    public class CreateCategoryCommandHandlerTests
    {
        private readonly Mock<ICrudService<CategoryInfo>> _categoryServiceMock;
        private readonly Mock<IClaimedService> _claimedServiceMock;
        private readonly CreateCategoryCommandHandler _handler;

        public CreateCategoryCommandHandlerTests()
        {
            _categoryServiceMock = new Mock<ICrudService<CategoryInfo>>();
            _claimedServiceMock = new Mock<IClaimedService>();
            _handler = new CreateCategoryCommandHandler(_categoryServiceMock.Object, _claimedServiceMock.Object);
        }

        [Fact]
        public async Task Handle_WithValidRequest_ShouldCreateAndReturnCategory()
        {
            // Arrange
            var request = new CreateCategoryCommand
            {
                CategoryName = "Electronics",
                CategoryDescription = "Electronic devices",
                IsActive = true
            };

            const string expectedUserId = "user-123";
            const string expectedRole = "Admin";
            const int expectedStoreId = 5;

            _claimedServiceMock.Setup(x => x.UserId).Returns(expectedUserId);
            _claimedServiceMock.Setup(x => x.Role).Returns(expectedRole);
            _claimedServiceMock.Setup(x => x.GetStoreIdAsync()).ReturnsAsync(expectedStoreId);

            _categoryServiceMock
                .Setup(x => x.InsertAsync(It.IsAny<CategoryInfo>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(request.CategoryName, result.CategoryName);
            Assert.Equal(request.CategoryDescription, result.CategoryDescription);
            Assert.Equal(expectedStoreId, result.StoreInfoId);
            Assert.Equal(request.IsActive, result.IsActive);
            Assert.Equal(expectedUserId, result.CreatedBy);

            _claimedServiceMock.Verify(x => x.GetStoreIdAsync(), Times.Once);
            _categoryServiceMock.Verify(x => x.InsertAsync(It.Is<CategoryInfo>(c =>
                c.CategoryName == request.CategoryName &&
                c.CategoryDescription == request.CategoryDescription &&
                c.StoreInfoId == expectedStoreId &&
                c.IsActive == request.IsActive &&
                c.CreatedBy == expectedUserId
            )), Times.Once);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Handle_WithNullOrWhiteSpaceCategoryName_ShouldThrowArgumentException(string? categoryName)
        {
            // Arrange
            var request = new CreateCategoryCommand
            {
                CategoryName = categoryName!,
                CategoryDescription = "Valid Description",
                IsActive = true
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(request, CancellationToken.None));

            Assert.Equal("Category name is required.", exception.Message);
            _categoryServiceMock.Verify(x => x.InsertAsync(It.IsAny<CategoryInfo>()), Times.Never);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("   ")]
        public async Task Handle_WithNullOrWhiteSpaceCategoryDescription_ShouldThrowArgumentException(string? categoryDescription)
        {
            // Arrange
            var request = new CreateCategoryCommand
            {
                CategoryName = "Valid Name",
                CategoryDescription = categoryDescription!,
                IsActive = true
            };

            // Act & Assert
            var exception = await Assert.ThrowsAsync<ArgumentException>(() =>
                _handler.Handle(request, CancellationToken.None));

            Assert.Equal("Category description is required.", exception.Message);
            _categoryServiceMock.Verify(x => x.InsertAsync(It.IsAny<CategoryInfo>()), Times.Never);
        }
    }
}