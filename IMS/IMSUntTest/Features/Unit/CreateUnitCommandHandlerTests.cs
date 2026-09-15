using System;
using System.Threading;
using System.Threading.Tasks;
using IMS.Application.Features.Unit.Command;
using IIMS.Application.Common.Interface;
using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using Moq;
using Xunit;

namespace IMSUntTest.Features.Unit.Command
{
    public class CreateUnitCommandHandlerTests
    {
        private readonly Mock<ICrudService<UnitInfo>> _unitInfoMock;
        private readonly Mock<IClaimedService> _claimedServiceMock;
        private readonly CreateUnitCommandHandler _handler;

        public CreateUnitCommandHandlerTests()
        {
            _unitInfoMock = new Mock<ICrudService<UnitInfo>>();
            _claimedServiceMock = new Mock<IClaimedService>();
            _handler = new CreateUnitCommandHandler(_unitInfoMock.Object, _claimedServiceMock.Object);
        }

        [Fact]
        public async Task Handle_ValidCommand_InsertsAndReturnsUnitInfo()
        {
            // Arrange
            var command = new CreateUnitCommand
            {
                UnitName = "Kilogram",
                IsActive = true
            };

            var expectedUserId = "user-123";
            _claimedServiceMock.Setup(x => x.UserId).Returns(expectedUserId);
            
            _unitInfoMock
                .Setup(x => x.InsertAsync(It.IsAny<UnitInfo>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Kilogram", result.UnitName);
            Assert.True(result.IsActive);
            Assert.Equal(expectedUserId, result.CreatedBy);
            Assert.NotEqual(default(DateTime), result.CreatedDate);

            _claimedServiceMock.Verify(x => x.UserId, Times.Once);
            _unitInfoMock.Verify(x => x.InsertAsync(It.Is<UnitInfo>(u => 
                u.UnitName == "Kilogram" && 
                u.IsActive && 
                u.CreatedBy == expectedUserId
            )), Times.Once);
        }

        [Fact]
        public async Task Handle_DependencyFailure_ThrowsException()
        {
            // Arrange
            var command = new CreateUnitCommand
            {
                UnitName = "Liter",
                IsActive = false
            };

            _claimedServiceMock.Setup(x => x.UserId).Returns("user-456");

            _unitInfoMock
                .Setup(x => x.InsertAsync(It.IsAny<UnitInfo>()))
                .ThrowsAsync(new InvalidOperationException("Database error"));

            // Act & Assert
            await Assert.ThrowsAsync<InvalidOperationException>(() => 
                _handler.Handle(command, CancellationToken.None));

            _unitInfoMock.Verify(x => x.InsertAsync(It.IsAny<UnitInfo>()), Times.Once);
        }
    }
}