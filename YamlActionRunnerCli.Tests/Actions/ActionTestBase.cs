namespace YamlActionRunnerCli.Tests.Actions
{
    /// <summary>
    /// Base class for all action-related tests.
    /// Sets up a shared Scope and a TestCorrelator for capturing log output.
    /// </summary>
    public abstract class ActionTestBase
    {
        protected Scope _scope;
        protected ITestCorrelatorContext _logContext;
        protected Mock<IAction> _mockAction1;
        protected Mock<IAction> _mockAction2;

        [SetUp]
        public virtual void SetUp()
        {
            // Set up the TestCorrelator for capturing logs
            _logContext = TestCorrelator.CreateContext();
            
            // Configure the static LoggerManager to use the test sink
            

            // Initialize a clean scope for each test
            _scope = new Scope
            {
                Logger = LoggerManager.Instance.Logger,
                Variables = new Variables()
            };

            // Initialize common mocks
            _mockAction1 = new Mock<IAction>();
            _mockAction2 = new Mock<IAction>();
        }

        [TearDown]
        public virtual void TearDown()
        {
            _logContext.Dispose();
        }
    }
}