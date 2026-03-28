using Microsoft.Extensions.Logging;
using Moq;
using SharedLogic.Caching.Services;
using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Models.Cells;
using SharedLogic.Models.Enums;
using SharedLogic.Motion;
using SharedLogic.Respiration.Services;

namespace UnitTests
{
    public class MotionTests
    {
        [Fact]
        public async Task MotionDepletesATP()
        {
            // Arrange
            var redisCacheService = new Mock<IRedisCacheService>();
            var cacheManagementService = new Mock<CacheManagementService<Myocyte>>("testOrgan", redisCacheService.Object);

            cacheManagementService.Setup(o => o.SetCellToCacheAsync())

            var cell = new Myocyte();

            var logger = new Mock<ILogger<MotionService>>();
            var motionService = new MotionService(cacheManagementService.Object, logger.Object);

            // Act
            await motionService.PerformMotionAsync(atpThreshold: 5);

            // Assert
            Assert.Equal(5, cell.NutrientConcentrations.ATPCount);
        }
    }
}
