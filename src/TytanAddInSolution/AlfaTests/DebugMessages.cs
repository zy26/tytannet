using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Pretorianie.Tytan.Core.DbgView;
using Pretorianie.Tytan.Core.DbgView.Sources;

namespace AlfaTests
{
    /// <summary>
    /// Summary description for DebugMessages
    /// </summary>
    [TestClass]
    public class DebugMessages
    {
        private TestContext testContextInstance;
        private int numberOfMessagesReceived;

        [DllImport("kernel32")]
        private static extern void OutputDebugString(string message);

        [TestInitialize]
        public void TestInitialize()
        {
            numberOfMessagesReceived = 0;
            DebugViewMonitor.ReceivedMessage += ReceivedDebugMessage;
            if (!DebugViewMonitor.Start())
                throw new ApplicationException("Can not start capturing debug messages.");
        }

        [TestCleanup]
        public void TestCleanUp()
        {
            DebugViewMonitor.Stop();
            DebugViewMonitor.ReceivedMessage -= ReceivedDebugMessage;
        }

        /// <summary>
        /// Sleep for some time to give the messages chance to be captured.
        /// </summary>
        private void WaitForResults()
        {
            Thread.Sleep(1200);
        }

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

        [DataSource("Microsoft.VisualStudio.TestTools.DataSource.CSV", "|DataDirectory|\\DebugTrace.csv", "DebugTrace#csv", DataAccessMethod.Sequential)]
        [DeploymentItem("AlfaTests\\DataSources\\DebugTrace.csv")]
        [TestMethod]
        public void TraceCapture()
        {
            string m = TestContext.DataRow["Message"].ToString().Replace("\\r", "\r").Replace("\\n", "\n");

            // send message to the DebugView via shared memory:
            OutputDebugString(m);
            WaitForResults();
            Assert.IsTrue(numberOfMessagesReceived > 0, "Number of debug messages received should be greater than zero!");
        }

        void ReceivedDebugMessage(IList<DebugViewData> items)
        {
            DebugViewMonitor.Stop();

            // write all captured messages:
            if (items != null)
            {
                foreach (DebugViewData d in items)
                    Trace.WriteLine(string.Format("{0}:{1} - {2} - {3}", d.PID, d.ProcessName, d.CreationTime, d.Message));
                
                numberOfMessagesReceived += items.Count;
            }

            DebugViewMonitor.Start();
        }

        [TestMethod]
        public void CaptureEmptyMessage()
        {
            OutputDebugString("");
            OutputDebugString(string.Empty);
            OutputDebugString(null);
            WaitForResults();

            Assert.IsTrue(numberOfMessagesReceived == 0, "No message should be received!");
        }

        [TestMethod]
        public void CaptureMessageFromCustomSource()
        {
            UserDebugSource s = new UserDebugSource("Custom Broadcaster", "My custom source");
            DebugViewMonitor.AddSource(s, false);

            s.Start();
            s.Write("Message1");
            s.Write("Message2\r\nMessage2a\r\n\r\nMessage2b");
            WaitForResults();

            Assert.IsTrue(numberOfMessagesReceived > 0, "Number of received debug messages should be greater than zero!");
        }
    }
}
