using SharedLogic.Models.Cells;
using SharedLogic.Models.Enzymes;
using SharedLogic.Respiration.Services;

namespace UnitTests.Respiration
{
    public class RespirationTypeSelectorServiceTests
    {
        [Fact]
        public void CanDoAerobicGlucoseRespirationWithDefaultConcentrations()
        {
            // Arrange
            var cell = new Cell();
            var respirationTypeSelectorService = new RespirationTypeSelectorService<Cell>(cell);

            // Act
            var result = respirationTypeSelectorService.CanDoAerobicGlucoseRespiration();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CannotDoAerobicGlucoseRespirationWithInsufficientOxygen()
        {
            // Arrange
            var cell = new Cell();
            cell.NutrientConcentrations.OxygenCount = 5;
            var respirationTypeSelectorService = new RespirationTypeSelectorService<Cell>(cell);

            // Act
            var result = respirationTypeSelectorService.CanDoAerobicGlucoseRespiration();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CannotDoAerobicGlucoseRespirationWithInsufficientGlucose()
        {
            // Arrange
            var cell = new Cell();
            cell.NutrientConcentrations.GlucoseCount = 0;
            var respirationTypeSelectorService = new RespirationTypeSelectorService<Cell>(cell);

            // Act
            var result = respirationTypeSelectorService.CanDoAerobicGlucoseRespiration();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CannotDoAerobicGlucoseRespirationWithATPSynthase()
        {
            // Arrange
            var cell = new Cell();
            cell.Enzymes = new List<Enzyme>();
            var respirationTypeSelectorService = new RespirationTypeSelectorService<Cell>(cell);

            // Act
            var result = respirationTypeSelectorService.CanDoAerobicGlucoseRespiration();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CanDoLipidMetabolismWithDefaultConcentrations()
        {
            // Arrange
            var cell = new Cell();
            var respirationTypeSelectorService = new RespirationTypeSelectorService<Cell>(cell);

            // Act
            var result = respirationTypeSelectorService.CanDoLipidMetabolism();

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void CannotDoLipidMetabolismWithInsufficientFattyAcidConcentration()
        {
            // Arrange
            var cell = new Cell();
            cell.NutrientConcentrations.FattyAcidsCount = 0;
            var respirationTypeSelectorService = new RespirationTypeSelectorService<Cell>(cell);

            // Act
            var result = respirationTypeSelectorService.CanDoLipidMetabolism();

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void CannotDoLipidMetabolismWithoutATPSynthaseOrLipase()
        {
            // Arrange
            var cell = new Cell();
            cell.Enzymes = new List<Enzyme>();
            var respirationTypeSelectorService = new RespirationTypeSelectorService<Cell>(cell);

            // Act
            var result = respirationTypeSelectorService.CanDoLipidMetabolism();

            // Assert
            Assert.False(result);
        }
    }
}
