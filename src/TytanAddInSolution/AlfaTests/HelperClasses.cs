using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pretorianie.Tytan.Core.Helpers;

namespace AlfaTests
{
    /// <summary>
    /// Summary description for HelperClasses
    /// </summary>
    [TestClass]
    public class HelperClasses
    {
        private TestContext testContextInstance;
        private Version remoteVersion;
        private ManualResetEvent manualEvent;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        /// <summary>
        /// Initialize all data object with their default values before execution of any test.
        /// </summary>
        [TestInitialize]
        public void TestInitialize()
        {
            manualEvent = new ManualResetEvent(false);
            remoteVersion = null;
        }

        /// <summary>
        /// Ask for the version info from the remote server and then validate if it is valid.
        /// </summary>
        [TestMethod]
        [Timeout(5000)]
        [Description("Asks for the latest version on the server.")]
        public void VersionHelper_GetVersion()
        {
            Version lowestAcceptableVersion = new Version("0.17");

            Assert.IsTrue(VersionHelper.CurrentVersion > lowestAcceptableVersion,
                          "Invalid version of current add-in. It should be greater!");

            VersionHelper.CheckVersion(GetVersionFromServer);
            manualEvent.WaitOne(4000);

            Trace.WriteLine(string.Format("Current version: {0}", VersionHelper.CurrentVersion));
            Trace.WriteLine(string.Format("Version on the server: {0}", remoteVersion));

            Assert.AreNotEqual(remoteVersion, null, "Invalid version info received from server");
            Assert.IsTrue(remoteVersion > lowestAcceptableVersion, "Remote version is too old.");
        }

        private void GetVersionFromServer(Version currentVersion, Version newVersion, string navigationURL)
        {
            remoteVersion = newVersion;
            manualEvent.Set();
        }
    }
}
