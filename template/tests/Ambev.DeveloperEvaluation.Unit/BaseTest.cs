using Ambev.DeveloperEvaluation.Domain.Common;
using Ambev.DeveloperEvaluation.Domain.Repositories;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit
{
    public abstract class BaseTest<T> where T : BaseEntity
    {
        IRepository<T>? _repository;
        public IRepository<T> Repository {
            get {
                _repository ??= CreateRepositoryInstance();

                return _repository;
            }}

        [Fact]
        public async Task ShouldIncrementSequentiallyWhenCreated()
        {
            for (var i = 1; i <= 2; ++i)
            {
                // Arrange
                var entity = CreateEntityDefaultInstance();
                await Repository.CreateAsync(entity);

                // Act
                var result = 1;// Repository.GetLastNumber();

                // Assert
                Assert.Equal(i, entity.Number);
                Assert.Equal(i, result);
            }
        }

        public abstract T CreateEntityDefaultInstance();

        public abstract IRepository<T> CreateRepositoryInstance();
    }
}
