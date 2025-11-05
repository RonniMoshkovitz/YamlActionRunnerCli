using System.Diagnostics;
using YamlActionRunnerCli.ActionManagement.Actions;

namespace YamlActionRunnerCli.Tests.Actions
{
    [TestFixture]
    public class DelayActionTests : ActionTestBase
    {
        [TestCase(100),
         TestCase(200)]
        public void Run_WithPositiveDuration_PausesForCorrectTimeRange(int durationMs)
        {
            // Arrange
            var action = new DelayAction
            {
                Duration = durationMs
            };
            var stopwatch = new Stopwatch();
            
            // overhead to account for OS thread 
            var upperBoundMs = durationMs + 40; 
            
            // small buffer for scheduling inaccuracies.
            var lowerBoundMs = durationMs - 10;

            // Act
            stopwatch.Start();
            action.Run(_scope);
            stopwatch.Stop();

            // Assert
            Assert.That(stopwatch.ElapsedMilliseconds, Is.GreaterThanOrEqualTo(lowerBoundMs), "Delay was significantly shorter than expected.");
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThanOrEqualTo(upperBoundMs), "Delay was significantly longer than expected.");
        }

        [Test(Description = "Test the edge case where duration is 0")]
        public void Run_WithZeroDuration_ReturnsImmediately()
        {
            // Arrange
            var action = new DelayAction
            {
                Duration = 0
            };
            var stopwatch = new Stopwatch();
            
            // overhead to account for OS thread 
            var upperBoundMs = 40; 

            // Act
            stopwatch.Start();
            action.Run(_scope);
            stopwatch.Stop();

            // Assert
            Assert.That(stopwatch.ElapsedMilliseconds, Is.LessThanOrEqualTo(upperBoundMs));
        }
    }
}

