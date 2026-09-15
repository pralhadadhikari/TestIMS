using System.Threading;
using System.Threading.Tasks;
using IMS.Application.Features.Categories.Command.DeleteCategories;
using IMS.Infrastructure.IRepository;
using IMS.Models.Entity;
using Moq;
using Xunit;

namespace IMSUntTest.Features.Categories.Command.DeleteCategories
{
    public class DeleteCategoryCommandHandlerTests
    {
        private readonly Mock<ICrudService<CategoryInfo>> _categoryServiceMock;
        private readonly DeleteCategoryCommandHandler _handler;

        public DeleteCategoryCommandHandlerTests()
        {
            _categoryServiceMock = new Mock<ICrudService<CategoryInfo>>();
            _handler = new DeleteCategoryCommandHandler(_categoryServiceMock.Object);
        }

        [Fact]
        public async Task Handle_CategoryExists_DeletesCategoryAndReturnsTrue()
        {
            // Arrange
            var command = new DeleteCategoryCommand { Id = 1 };
            var category = new CategoryInfo { Id = 1, CategoryName = "Electronics" };

            _categoryServiceMock
                .Setup(x => x.GetAsync(command.Id))
                .ReturnsAsync(category);

            _categoryServiceMock
                .Setup(x => x.Delete(category));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _categoryServiceMock.Verify(x => x.GetAsync(command.Id), Times.Once);
            _categoryServiceMock.Verify(x => x.Delete(category), Times.Once);
        }

        [Fact]
        public async Task Handle_CategoryDoesNotExist_ReturnsFalseAndDoesNotDelete()
        {
            // Arrange
            var command = new DeleteCategoryCommand { Id = 99 };

            _categoryServiceMock
                .Setup(x => x.GetAsync(command.Id))
                .ReturnsAsync((CategoryInfo?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.False(result);
            _categoryServiceMock.Verify(x => x.GetAsync(command.Id), Times.Once);
            _categoryServiceMock.Verify(x => x.Delete(It.IsAny<CategoryInfo>()), Times.Never);
        }
    }
}