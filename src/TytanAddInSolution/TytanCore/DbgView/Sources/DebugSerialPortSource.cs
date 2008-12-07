using System.IO.Ports;
using System.Text;

namespace Pretorianie.Tytan.Core.DbgView.Sources
{
    /// <summary>
    /// Class that reads debug messages from serial port.
    /// </summary>
    public class DebugSerialPortSource : IDbgSource
    {
        private readonly string name;
        private readonly string description;
        private readonly SerialPort port;

        /// <summary>
        /// Definition of the callback function that will receive debug messages.
        /// </summary>
        public event DbgDataEventHandler DataReceived;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public DebugSerialPortSource(string name, BaudRates baudRate, Encoding encoder, Parity parity, int dataBits, StopBits stopBits)
        {
            this.name = name;
            description = string.Format("{0} ({1} / {2}) Source", name, (int)baudRate, encoder.EncodingName);

            if (stopBits == StopBits.None)
                port = new SerialPort(name, (int)baudRate, parity, dataBits);
            else
                port = new SerialPort(name, (int)baudRate, parity, dataBits, stopBits);
            port.Encoding = encoder;
            port.DataReceived += InternalDataReceived;
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public DebugSerialPortSource(string name)
            : this(name, BaudRates.X115200, Encoding.ASCII, Parity.None, 8, StopBits.None)
        {
        }

        #region Properties

        /// <summary>
        /// Gets the name of current serial port.
        /// </summary>
        public string Name
        {
            get { return name; }
        }

        /// <summary>
        /// Checks if given serial port is opened and valid for further operations.
        /// </summary>
        public bool IsValid
        {
            get { return port != null && port.IsOpen; }
        }


        /// <summary>
        /// Gets the module name of given source.
        /// </summary>
        public string Module
        {
            get { return name; }
        }

        /// <summary>
        /// Gets the description of given source.
        /// </summary>
        public string Description
        {
            get { return description; }
        }

        #endregion

        /// <summary>
        /// Opens specified COM port and starts listening for data.
        /// </summary>
        public void Open()
        {
            if (port != null)
                port.Open();
        }

        /// <summary>
        /// Provides info if given source can be disabled at runtime by the user.
        /// </summary>
        public bool CanConfigureAtRuntime
        {
            get { return true; }
        }

        /// <summary>
        /// Reinitialize and start processing of messages.
        /// </summary>
        public void Start()
        {
            Open();
        }

        /// <summary>
        /// Stops monitoring for data.
        /// </summary>
        public void Close()
        {
            if (port != null)
                port.Close();
        }

        private void InternalDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // end of transmission ?
            if (e.EventType == SerialData.Eof)
                return;

            if (DataReceived != null)
                // read whole data and
                // convert to the actual message understandable by the user and broadcast it:
                DataReceived(this, 0, port.ReadExisting());
            else
                port.DiscardInBuffer();
        }
    }
}