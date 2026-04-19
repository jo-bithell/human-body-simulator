using Lungs.Models;
using Lungs.Services;
using Microsoft.Extensions.Logging;
using Moq;
using SharedLogic.Caching.Services;
using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Models.Cells;

namespace Tests.Diffusion
{
    public class AirDiffusionServiceTests : IClassFixture<RedisFixture>
    {
        private readonly ICacheManagementService<AlveolarCell> _cacheManagementService;
        public AirDiffusionServiceTests(RedisFixture redisFixture)
        {
            var redisCacheService = new RedisCacheService(redisFixture.Connection);
            _cacheManagementService = new CacheManagementService<AlveolarCell>("test", redisCacheService);
        }

        [Fact]
        public async Task CarbonDioxideDiffusesFromCellToAir()
        {
            // Arrange
            var cell = new AlveolarCell();
            cell.NutrientConcentrations.CarbonDioxideCount = 10;
            var air = new Air();
            air.CarbonDioxideCount = 0;
            var logger = new Mock<ILogger<AirDiffusionService>>();
            await _cacheManagementService.SetCellToCacheAsync(cell);
            var diffusionService = new AirDiffusionService(_cacheManagementService, air, logger.Object);
            // Act
            await diffusionService.DiffuseAsync();
            // Assert
            var cellInCache = await _cacheManagementService.GetCellFromCacheAsync();
            Assert.Equal(10, cellInCache.NutrientConcentrations.CarbonDioxideCount);
            Assert.Equal(0, air.CarbonDioxideCount);
        }

        [Fact]
        public async Task OxygenDiffusesFromCellToAir()
        {
            // Arrange
            var cell = new AlveolarCell();
            cell.NutrientConcentrations.CarbonDioxideCount = 10;
            var air = new Air();
            air.CarbonDioxideCount = 0;
            var logger = new Mock<ILogger<AirDiffusionService>>();
            await _cacheManagementService.SetCellToCacheAsync(cell);
            var diffusionService = new AirDiffusionService(_cacheManagementService, air, logger.Object);
            // Act
            await diffusionService.DiffuseAsync();
            // Assert
            var cellInCache = await _cacheManagementService.GetCellFromCacheAsync();
            Assert.Equal(10, cellInCache.NutrientConcentrations.OxygenCount);
            Assert.Equal(0, air.CarbonDioxideCount);
        }
    }
}
