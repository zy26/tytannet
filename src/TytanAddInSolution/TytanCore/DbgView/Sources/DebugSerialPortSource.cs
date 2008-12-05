using System.IO.Ports;
using System.Text;

namespace Pretorianie.Tytan.Core.DbgView.Sources
{
    /// <summary>
    /// Class that reads debug messages from serial port.
    /// </summary>
    public class SerialPortDebugReader
    {
        /// <summary>
        /// Definition of the callback function that will receive debug messages.
        /// </summary>
        public delegate void ReceiveHandler(SerialPortDebugReader port, string message);

        /// <summary>
        /// Event fired when new text has been received via serial port.
        /// </summary>
        public event ReceiveHandler ReceivedMessage;

        private readonly string name;
        private readonly uint baudRate;
        private readonly Encoding encoder;
        private SerialPort port;
        private byte[] buffer;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public SerialPortDebugReader(string name, uint baudRate, Encoding encoder)
        {
            this.name = name;
            this.baudRate = baudRate;
            this.encoder = encoder;
            buffer = new byte[4096];
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public SerialPortDebugReader(string name)
            : this(name, 115200, Encoding.ASCII)
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

        #endregion

        public void Open()
        {
            if (port == null)
            {
                port = new SerialPort(name, (int) baudRate);
                port.DataReceived += DataReceived;

                port.Open();
            }
        }

        /// <summary>
        /// Stops monitoring for data.
        /// </summary>
        public void Close()
        {
            if (port != null)
            {
                port.Close();
                port.Dispose();
                port = null;
            }
        }

        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            // end of transmission ?
            if (e.EventType == SerialData.Eof)
                return;

            int count = port.BytesToRead;

            // read whole data:
            if (count > 0)
            {
                if (ReceivedMessage != null)
                {
                    if (count > buffer.Length)
                        buffer = new byte[count];

                    port.Read(buffer, 0, count);

                    // convert to the actual message understandable by the user
                    // and broadcast it:
                    ReceivedMessage(this, encoder.GetString(buffer, 0, count));
                }
                else
                {
                    port.DiscardInBuffer();
                }
            }
        }
    }
}