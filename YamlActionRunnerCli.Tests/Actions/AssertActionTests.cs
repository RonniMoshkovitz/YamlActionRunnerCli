namespace YamlActionRunnerCli.Tests.Actions
{
    [TestFixture]
    public class AssertActionTests : ActionTestBase
    {
        [TestCase("1 == 1"),
        TestCase("1 + 3 == 4"),
        TestCase(" 100 != 5"),
        TestCase("\"hello\" == \"hello\""),
        TestCase("true")]
        public void Run_WithTrueCondition_Succeeds(string condition)
        {
            // Arrange
            var action = new AssertAction { Condition = condition };

            // Act & Assert
            Assert.DoesNotThrow(() => action.Run(_scope));
        }

        [TestCase("\"yes\" == \"no\""),
         TestCase("1 + 1 == 3"),
         TestCase("false")]
        public void Run_WithFalseCondition_ThrowsFailedAssertionException(string condition)
        {
            // Arrange
            var action = new AssertAction { Condition = condition };

            // Act & Assert
            Assert.Throws<FailedAssertionException>(() => action.Run(_scope));
        }

        [TestCase("1 =="),
         TestCase("1 + 3 = 4"),
         TestCase("invalid C# syntax")]
        public void Run_WithInvalidSyntax_ThrowsInvalidConditionException(string condition)
        {
            // Arrange
            var action = new AssertAction { Condition = condition };

            // Act & Assert
            Assert.Throws<InvalidConditionException>(() => action.Run(_scope));
        }
    }
}