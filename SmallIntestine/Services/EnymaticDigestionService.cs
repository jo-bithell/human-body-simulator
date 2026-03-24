using SharedLogic.Caching.Services.Interfaces;
using SharedLogic.Models.Enums;
using SharedLogic.Models.Enzymes;
using SmallIntestine.Models;

namespace SmallIntestine.Services
{
    internal class EnymaticDigestionService
    {
        private readonly ICacheManagementService<Enterocyte> _cacheManagementService;
        public EnymaticDigestionService(ICacheManagementService<Enterocyte> cacheManagementService) 
        {
            _cacheManagementService = cacheManagementService;
        }

        public async Task PerformEnzymaticDigestionAsync(NutrientType nutrientType, int nutrientCount)
        {
            await _cacheManagementService.PerformFunctionAsync(async (Enterocyte cell) =>
            {
                switch (nutrientType)
                {
                    case NutrientType.Protein:
                        BreakDownForProteins(cell);
                        break;
                    default:
                        throw new InvalidOperationException($"Unsupported nutrient type: {nutrientType}");
                }
                await Task.CompletedTask;
            });
        }

        private void BreakDownForProteins(Enterocyte cell)
        {
            throw new NotImplementedException();
        }
    }
}
