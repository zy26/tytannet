using System;
using System.Collections.Generic;
using System.Text;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.DbgView.Sources;
using System.IO.Ports;

namespace Pretorianie.Tytan.Forms
{
    public partial class DebugViewNewSerialForm : Core.BaseForms.BasePackageForm
    {
        private List<KeyValuePair<string, Encoding>> encodings;

        public DebugViewNewSerialForm()
        {
            InitializeComponent();

            // add baud rates:
            cmbBaudRate.Items.Clear();
            foreach (BaudRates b in Enum.GetValues(typeof(BaudRates)))
                cmbBaudRate.Items.Add((int)b);

            // and select the last one:
            if (cmbBaudRate.Items.Count > 0)
                cmbBaudRate.SelectedIndex = cmbBaudRate.Items.Count - 4;

            // add encodings:
            encodings = new List<KeyValuePair<string, Encoding>>();
            encodings.Add(new KeyValuePair<string, Encoding>("ASCII", Encoding.ASCII));
            encodings.Add(new KeyValuePair<string, Encoding>("UTF7", Encoding.UTF7));
            encodings.Add(new KeyValuePair<string, Encoding>("UTF8", Encoding.UTF8));
            encodings.Add(new KeyValuePair<string, Encoding>("Unicode", Encoding.Unicode));
            encodings.Add(new KeyValuePair<string, Encoding>("UTF32", Encoding.UTF32));
            encodings.Add(new KeyValuePair<string, Encoding>("Big-Endian Unicode", Encoding.BigEndianUnicode));

            foreach (KeyValuePair<string, Encoding> x in encodings)
                cmbEncodings.Items.Add(x.Key);
            cmbEncodings.SelectedIndex = 0;

            // add parity:
            foreach (Parity p in Enum.GetValues(typeof(Parity)))
                cmbParity.Items.Add(p);
            cmbParity.SelectedIndex = 0;

            // add stop bits:
            foreach (StopBits s in Enum.GetValues(typeof(StopBits)))
                cmbStopBits.Items.Add(s);
            cmbStopBits.SelectedIndex = 1;

            // add data bits:
            cmbDataBits.Items.Add("7");
            cmbDataBits.Items.Add("8");
            cmbDataBits.SelectedIndex = 1;

            // add flow control:
            cmbFlow.Items.Add("None");
            cmbFlow.SelectedIndex = 0;
        }

        public void InitializeUI()
        {
            // add serial port names:
            cmbPortNames.Items.Clear();
            foreach (string n in ComHelper.GetNames())
                cmbPortNames.Items.Add(n);

            // select the last one:
            if (cmbPortNames.Items.Count > 0)
                cmbPortNames.SelectedIndex = 0;

            ActiveControl = cmbPortNames;
        }

        #region Properties

        /// <summary>
        /// Gets the name of currently selected serial port.
        /// </summary>
        public string PortName
        {
            get { return (string)cmbPortNames.Items[cmbPortNames.SelectedIndex]; }
        }

        /// <summary>
        /// Gets the currently selected baud-rate for serial port.
        /// </summary>
        public BaudRates PortBaudRate
        {
            get { return (BaudRates)cmbBaudRate.Items[cmbBaudRate.SelectedIndex]; }
        }

        /// <summary>
        /// Get selected data encoding for given serial port.
        /// </summary>
        public Encoding PortEncoding
        {
            get { return encodings[cmbEncodings.SelectedIndex].Value; }
        }

        /// <summary>
        /// Gets the parity of given serial port.
        /// </summary>
        public Parity PortParity
        {
            get { return (Parity)cmbParity.Items[cmbParity.SelectedIndex]; }
        }

        /// <summary>
        /// Gets the stop-bits of given serial port.
        /// </summary>
        public StopBits PortStopBits
        {
            get { return (StopBits)cmbStopBits.Items[cmbStopBits.SelectedIndex]; }
        }

        /// <summary>
        /// Gets the data-bits of given serial port.
        /// </summary>
        public int PortDataBits
        {
            get
            {
                int bits;

                if (int.TryParse(cmbDataBits.Text, out bits))
                    return bits;
                return 0;
            }
        }

        #endregion
    }
}
