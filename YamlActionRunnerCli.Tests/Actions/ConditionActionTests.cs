namespace YamlActionRunnerCli.Tests.Actions
{
    [TestFixture]
    public class ConditionActionTests : ActionTestBase
    {
        private ConditionAction _conditionAction;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();
            _conditionAction = new ConditionAction { Actions = [_mockAction1.Object] };
        }

        [TestCase("1 == 1"),
         TestCase("1 + 3 == 4"),
         TestCase(" 100 != 5"),
         TestCase("\"hello\" == \"hello\""),
         TestCase("true")]
        public void Run_WithTrueCondition_ExecutesNestedActions(string condition)
        {
            // Arrange
            _conditionAction.Condition = condition;

            // Act
            _conditionAction.Run(_scope);

            // Assert
            _mockAction1.Verify(a => a.Run(_scope), Times.Once());
        }

        [TestCase("\"yes\" == \"no\""),
         TestCase("1 + 1 == 3"),
         TestCase("false")]
        public void Run_WithFalseCondition_DoesNotExecuteNestedActions(string condition)
        {
            // Arrange
            _conditionAction.Condition = condition;

            // Act
            _conditionAction.Run(_scope);

            // Assert
            _mockAction1.Verify(a => a.Run(_scope), Times.Never());
        }

        [TestCase("1 =="),
         TestCase("1 + 3 = 4"),
         TestCase("invalid C# syntax")]
        public void Run_WithInvalidCondition_ThrowsInvalidConditionException(string condition)
        {
            // Arrange
            _conditionAction.Condition = condition;

            // Act & Assert
            Assert.Throws<InvalidConditionException>(() => _conditionAction.Run(_scope));
        }
    }
}