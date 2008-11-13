using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pretorianie.Tytan.Core.CustomAddIn;
using Pretorianie.Tytan.Core.Interfaces;
using Rhino.Mocks;
using System.Reflection;

namespace AlfaTests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class ActionsManagement
    {
        #region Context

        private MockRepository mock;
        private int actionID;
        private int initializeOnceCounter;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext { get; set; }

        #endregion

        #region Test Initialize / CleanUp

        /// <summary>
        /// Create new mock repository.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            mock = new MockRepository();
            initializeOnceCounter = 0;
            actionID = 100;
        }

        /// <summary>
        /// Validate data access and function call to the mock objects and destroy the mock repository.
        /// </summary>
        [TestCleanup]
        public void TestCleanUp()
        {
            mock.VerifyAll();
            mock = null;
        }

        #endregion

        #region Delegates

        private delegate void InitializeDelegate(IPackageEnvironment env, IMenuCreator mc);

        #endregion

        #region Helpers

        /// <summary>
        /// Creates mock <c>IPackageAction</c> for Initialize() methods calls validation.
        /// </summary>
        private IPackageAction CreatePackageAction()
        {
            var action = mock.DynamicMock<IPackageAction>();

            action.Initialize(null, null);
            LastCall.Repeat.Any().IgnoreArguments().Do(new InitializeDelegate(InitializeOnceCheck));

            return action;
        }

        private void InitializeOnceCheck(IPackageEnvironment env, IMenuCreator mc)
        {
            // check the number of Initialize() method calls:
            initializeOnceCounter++;
        }

        private void ThrowCounterException(int maxValue)
        {
            if (initializeOnceCounter > maxValue)
                throw new ApplicationException("Invalid number of executions for IPackageAction.Initialize() method");
        }

        /// <summary>
        /// Creates <c>IPackageAction</c> object validating calls from <c>IPackageEnvironment</c> object.
        /// </summary>
        private IPackageAction CreatePackageActionForManager()
        {
            var action = CreatePackageAction();

            // declare actions that can be performed over the mock object:
            Expect.Call(action.Configuration).Repeat.Any().Return(null);
            Expect.Call(action.ID).Repeat.Any().Return(++actionID);

            return action;
        }

        /// <summary>
        /// Creates Visual Studio's DTE2 object for use by <c>IPackageEnvironment</c>.
        /// </summary>
        private DTE2 CreateDTE()
        {
            var dte = mock.DynamicMock<DTE2>();

            Expect.Call(dte.LocaleID).Repeat.Any().Return(1033);

            return dte;
        }

        /// <summary>
        /// Creates Visual Studio's <c>AddIn</c> object for use by <c>IPackageEnvironment</c>
        /// </summary>
        private AddIn CreateAddIn()
        {
            var addIn = mock.DynamicMock<AddIn>();

            return addIn;
        }

        #endregion

        [TestMethod]
        [Description("This test creates an abstract action an tries to call Initialize() 2 times. Expected exception: ApplicationException().")]
        [ExpectedException(typeof(ApplicationException), "Expected ApplicationException to be thrown")]
        public void PackageAction_InitializeFewTimes()
        {
            IPackageAction action = CreatePackageAction();

            mock.ReplayAll();

            // try to initialize few times:
            action.Initialize(null, null);
            action.Initialize(null, null);
            action.Destroy();

            ThrowCounterException(1);
        }

        [TestMethod]
        [Description("This test creates an AddIn manager, adds few actions and calls ApplicationInit() several times, expecting only one call to IPackageAction.Initialize() to be performed.")]
        public void AddInManager_CallInitializeOnceForAction()
        {
            var a1 = CreatePackageActionForManager();
            var a2 = CreatePackageActionForManager();
            var dte = CreateDTE();
            var addIn = CreateAddIn();

            mock.ReplayAll();

            var manager = new CustomAddInManager(dte, addIn, null, Assembly.GetExecutingAssembly());

            manager.Add(a1);
            manager.Add(a2);

            // initialize actions - only the first one should be executed
            // the rest should be discarded by the manager itself:
            manager.ApplicationInit(true);
            manager.ApplicationInit(true);

            // release manager's resources:
            manager.ApplicationExit(true);

            // check if the initialization has been called at most 2-times:
            ThrowCounterException(2);
        }
    }
}
