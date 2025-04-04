using Ambev.DeveloperEvaluation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Ambev.DeveloperEvaluation.Unit
{
    internal static class BaseTestHelpers
    {
        internal static DefaultContext GetDefaultContextInstanceInMemoryDatabase()
        {
            var dbContextOptions = new DbContextOptionsBuilder<DefaultContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options;
            return new DefaultContext(dbContextOptions);
        }
    }
}