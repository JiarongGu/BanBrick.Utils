using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace BanBrick.Utils.Test
{
    public class ValidationTestModel
    {
        [Range(1, 10)]
        public int Id { get; set; }

        [Required]
        public string Value { get; set; }
    }

    public class ModelValidatorTest
    {
        [Fact]
        public void Should_Return_ValidationResults_When_Detect_ValidationErrors()
        {
            // Arrange
            var model = new ValidationTestModel();

            // Act
            var validationResults = ModelValidator.Validate(model);

            // Assert
            Assert.Equal(2, validationResults.Count());
        }

        [Fact]
        public async Task Should_Work_Concurrently_When_ValidatingAsync()
        {
            // Arrange
            var model1 = new ValidationTestModel() { Id = 1 };
            var model2 = new ValidationTestModel() { Value = "Test" };

            // Act
            Func<IEnumerable<ValidationResult>> func1 = () => ModelValidator.Validate(model1);
            Func<IEnumerable<ValidationResult>> func2 = () => ModelValidator.Validate(model2);
            var validationResults = await Task.WhenAll(Task.Run(func1), Task.Run(func2));

            // Assert
            Assert.Equal(2, validationResults.Count());
        }

        [Fact]
        public void Should_Work_Faster_After_FirstValidation()
        {
            // Arrange
            var model = new ValidationTestModel();

            // Act
            var duration1 = GetActionDuration(() => ModelValidator.Validate(model));
            var duration2 = GetActionDuration(() => ModelValidator.Validate(model));

            // Assert
            Assert.True(duration1 > duration2);
        }

        private TimeSpan GetActionDuration(Action action)
        {
            var stopWatch = new Stopwatch();
            stopWatch.Start();
            action();
            stopWatch.Stop();
            return stopWatch.Elapsed;
        }
    }
}
