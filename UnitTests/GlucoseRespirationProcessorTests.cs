using SharedLogic.Models.Cells;
using SharedLogic.Models.Enums;
using SharedLogic.Respiration.Services;

namespace UnitTests
{
    public class GlucoseRespirationProcessorTests
    {
        private readonly GlucoseRespirationProcessor<Cell> _glucoseRespirationProcessor;
        private readonly Cell _cell;
        public GlucoseRespirationProcessorTests()
        {
            _cell = new Cell();
            _glucoseRespirationProcessor = new GlucoseRespirationProcessor<Cell>(_cell);
        }

        [Fact]
        public void GlucoseCountDecreasesByOne()
        {
            // Act
            _glucoseRespirationProcessor.Process();
            // Assert
            Assert.Equal(9, _cell.NutrientConcentrations.GlucoseCount);
        }

        [Fact]
        public void CarbonDioxideCountIncreasesBy6()
        {
            // Act
            _glucoseRespirationProcessor.Process();
            // Assert
            Assert.Equal(16, _cell.NutrientConcentrations.CarbonDioxideCount);
        }

        [Fact]
        public void ATPCountIncreasesBy34()
        {
            // Act
            _glucoseRespirationProcessor.Process();
            // Assert
            Assert.Equal(44, _cell.NutrientConcentrations.ATPCount);
        }

        [Fact]
        public void ATPNotIncreasedIfATPSynthaseInUse()
        {
            // Act
            _cell.Enzymes.First(o => o.EnzymeType == EnzymeType.ATPSynthase).InUse = true;
            var glucoseRespirationProcessor = new GlucoseRespirationProcessor<Cell>(_cell);
            glucoseRespirationProcessor.Process();
            // Assert
            Assert.Equal(10, _cell.NutrientConcentrations.ATPCount);
        }

        [Fact]
        public void ATPNotIncreasedIfATPSynthaseNotPresent()
        {
            // Act
            _cell.Enzymes.RemoveAll(o => o.EnzymeType == EnzymeType.ATPSynthase);
            var glucoseRespirationProcessor = new GlucoseRespirationProcessor<Cell>(_cell);
            glucoseRespirationProcessor.Process();
            // Assert
            Assert.Equal(10, _cell.NutrientConcentrations.ATPCount);
        }

        [Fact]
        public void OxygenCountDecreasesBy6()
        {
            // Act
            _glucoseRespirationProcessor.Process();
            // Assert
            Assert.Equal(4, _cell.NutrientConcentrations.OxygenCount);
        }
    }
}
