using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using IIMS.Application.Common.Interface;
using IMS.Application.Features.Products.Command;
using IMS.Infrastructure.IRepository;
using IMS.Infrastructure.Services;
using IMS.Models.Entity;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace IMSUntTest.Features.Products.Command
{
    public class CreateProductCommandHandlerTests
    {
        private readonly Mock<ICrudService<ProductInfo>> _productInfoMock;
        private readonly Mock<IClaimedService> _claimedServiceMock;
        private readonly Mock<IFileService> _fileServiceMock;
        private readonly CreateProductCommandHandler _handler;

        public CreateProductCommandHandlerTests()
        {
            _productInfoMock = new Mock<ICrudService<ProductInfo>>();
            _claimedServiceMock = new Mock<IClaimedService>();
            _fileServiceMock = new Mock<IFileService>();

            _handler = new CreateProductCommandHandler(
                _productInfoMock.Object,
                _claimedServiceMock.Object,
                _fileServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommandWithoutImage_ShouldCreateAndReturnProduct()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                CategoryInfoId = 1,
                ProductName = "Test Product",
                ProductDescription = "Test Description",
                UnitInfoId = 2,
                IsActive = true,
                ImageFile = null!
            };

            int expectedStoreId = 10;
            string expectedUserId = "user-123";

            _claimedServiceMock.Setup(x => x.GetStoreIdAsync()).ReturnsAsync(expectedStoreId);
            _claimedServiceMock.SetupGet(x => x.UserId).Returns(expectedUserId);
            
            _productInfoMock.Setup(x => x.InsertAsync(It.IsAny<ProductInfo>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(command.CategoryInfoId, result.CategoryInfoId);
            Assert.Equal(command.ProductName, result.ProductName);
            Assert.Equal(command.ProductDescription, result.ProductDescription);
            Assert.Equal(command.UnitInfoId, result.UnitInfoId);
            Assert.Equal(expectedStoreId, result.StoreInfoId);
            Assert.Equal(command.IsActive, result.IsActive);
            Assert.Equal(expectedUserId, result.CreatedBy);
            Assert.Null(result.ImageUrl);

            _claimedServiceMock.Verify(x => x.GetStoreIdAsync(), Times.Once);
            _fileServiceMock.Verify(x => x.SaveFileAsync(It.IsAny<IFormFile>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            _productInfoMock.Verify(x => x.InsertAsync(It.IsAny<ProductInfo>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidCommandWithImage_ShouldSaveImageAndCreateProduct()
        {
            // Arrange
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.Length).Returns(1024);

            var command = new CreateProductCommand
            {
                CategoryInfoId = 1,
                ProductName = "Test Product With Image",
                ProductDescription = "Test Description",
                UnitInfoId = 2,
                IsActive = true,
                ImageFile = fileMock.Object
            };

            int expectedStoreId = 15;
            string expectedUserId = "user-456";
            string expectedImageUrl = "/uploads/ProductImage/test.jpg";

            _claimedServiceMock.Setup(x => x.GetStoreIdAsync()).ReturnsAsync(expectedStoreId);
            _claimedServiceMock.SetupGet(x => x.UserId).Returns(expectedUserId);

            _fileServiceMock.Setup(x => x.SaveFileAsync(fileMock.Object, "ProductImage", It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedImageUrl);

            _productInfoMock.Setup(x => x.InsertAsync(It.IsAny<ProductInfo>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedImageUrl, result.ImageUrl);
            Assert.Equal(expectedStoreId, result.StoreInfoId);
            Assert.Equal(expectedUserId, result.CreatedBy);

            _fileServiceMock.Verify(x => x.SaveFileAsync(fileMock.Object, "ProductImage", It.IsAny<CancellationToken>()), Times.Once);
            _productInfoMock.Verify(x => x.InsertAsync(It.IsAny<ProductInfo>()), Times.Once);
        }

        [Fact]
        public async Task Handle_DependencyFailure_GetStoreIdAsyncThrowsException_ShouldPropagateException()
        {
            // Arrange
            var command = new CreateProductCommand
            {
                CategoryInfoId = 1,
                ProductName = "Test Product",
                ProductDescription = "Test Description",
                UnitInfoId = 2,
                IsActive = true
            };

            _claimedServiceMock.Setup(x => x.GetStoreIdAsync())
                .ThrowsAsync(new InvalidOperationException("Store not found"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            _productInfoMock.Verify(x => x.InsertAsync(It.IsAny<ProductInfo>()), Times.Never);
        }
    }
}