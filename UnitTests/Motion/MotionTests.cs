using Microsoft.Extensions.Logging;
using Moq;
using SharedLogic.Caching.Services;
using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Models.Cells;
using SharedLogic.Motion;

namespace UnitTests
{
    public class MotionTests : IClassFixture<RedisFixture>
    {
        private readonly ICacheManagementService<Myocyte> _cacheManagementService;
        public MotionTests(RedisFixture redisFixture) 
        {
            var redisCacheService = new RedisCacheService(redisFixture.Connection);
            _cacheManagementService = new CacheManagementService<Myocyte>("test", redisCacheService);
        }

        [Fact]
        public async Task MotionDepletesATP()
        {
            // Arrange
            var cell = new Myocyte();

            await _cacheManagementService.SetCellToCacheAsync(cell);

            var logger = new Mock<ILogger<MotionService>>();
            var motionService = new MotionService(_cacheManagementService, logger.Object);

            // Act
            await motionService.PerformMotionAsync(atpThreshold: 5);

            // Assert
            var cellInCache = await _cacheManagementService.GetCellFromCacheAsync();
            Assert.Equal(5, cellInCache.NutrientConcentrations.ATPCount);
        }

        [Fact]
        public async Task MotionDoesNotOccurIfInsufficientATP()
        {
            // Arrange
            var cell = new Myocyte();
            cell.NutrientConcentrations.ATPCount = 1;

            await _cacheManagementService.SetCellToCacheAsync(cell);

            var logger = new Mock<ILogger<MotionService>>();
            var motionService = new MotionService(_cacheManagementService, logger.Object);

            // Act
            await motionService.PerformMotionAsync(atpThreshold: 5);

            // Assert
            var cellInCache = await _cacheManagementService.GetCellFromCacheAsync();
            Assert.Equal(1, cellInCache.NutrientConcentrations.ATPCount);
        }

        [Fact]
        public async Task CorrectlyIdentifiesWhenCellCanPerformMotion()
        {
            // Arrange
            var cell = new Myocyte();
            cell.NutrientConcentrations.ATPCount = 5;

            await _cacheManagementService.SetCellToCacheAsync(cell);

            var logger = new Mock<ILogger<MotionService>>();
            var motionService = new MotionService(_cacheManagementService, logger.Object);

            // Act
            var result = await motionService.CanPerformMotionAsync(atpThreshold: 5);

            // Assert
            Assert.True(result);
        }
    }
}
