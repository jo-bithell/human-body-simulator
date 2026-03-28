using SharedLogic.Models.Cells;
using SharedLogic.Models.Enums;
using SharedLogic.Respiration.Services;

namespace UnitTests.Respiration
{
    public class LipidRespirationProcessorTests
    {
        private readonly LipidRespirationProcessor<Cell> _lipidRespirationProcessor;
        private readonly Cell _cell;
        public LipidRespirationProcessorTests()
        {
            _cell = new Cell();
            _lipidRespirationProcessor = new LipidRespirationProcessor<Cell>(_cell);
        }

        [Fact]
        public void FattyAcidCountDecreasesByOne()
        {
            // Act
            _lipidRespirationProcessor.Process();
            // Assert
            Assert.Equal(9, _cell.NutrientConcentrations.FattyAcidsCount);
        }

        [Fact]
        public void ATPCountIncreasesBy96()
        {
            // Act
            _lipidRespirationProcessor.Process();
            // Assert
            Assert.Equal(106, _cell.NutrientConcentrations.ATPCount);
        }

        [Fact]
        public void ATPNotIncreasedIfATPSynthaseInUse()
        {
            // Act
            _cell.Enzymes.First(o => o.EnzymeType == EnzymeType.ATPSynthase).InUse = true;
            var glucoseRespirationProcessor = new LipidRespirationProcessor<Cell>(_cell);
            glucoseRespirationProcessor.Process();
            // Assert
            Assert.Equal(10, _cell.NutrientConcentrations.ATPCount);
        }

        [Fact]
        public void ATPNotIncreasedIfATPSynthaseNotPresent()
        {
            // Act
            _cell.Enzymes.RemoveAll(o => o.EnzymeType == EnzymeType.ATPSynthase);
            var glucoseRespirationProcessor = new LipidRespirationProcessor<Cell>(_cell);
            glucoseRespirationProcessor.Process();
            // Assert
            Assert.Equal(10, _cell.NutrientConcentrations.ATPCount);
        }

        [Fact]
        public void ATPNotIncreasedIfLipaseInUse()
        {
            // Act
            _cell.Enzymes.First(o => o.EnzymeType == EnzymeType.Lipase).InUse = true;
            var glucoseRespirationProcessor = new LipidRespirationProcessor<Cell>(_cell);
            glucoseRespirationProcessor.Process();
            // Assert
            Assert.Equal(10, _cell.NutrientConcentrations.ATPCount);
        }

        [Fact]
        public void ATPNotIncreasedIfLipaseNotPresent()
        {
            // Act
            _cell.Enzymes.RemoveAll(o => o.EnzymeType == EnzymeType.Lipase);
            var glucoseRespirationProcessor = new LipidRespirationProcessor<Cell>(_cell);
            glucoseRespirationProcessor.Process();
            // Assert
            Assert.Equal(10, _cell.NutrientConcentrations.ATPCount);
        }
    }
}
