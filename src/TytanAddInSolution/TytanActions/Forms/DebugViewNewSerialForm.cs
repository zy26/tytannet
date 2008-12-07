using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Pretorianie.Tytan.Core.Helpers;
using Pretorianie.Tytan.Core.DbgView.Sources;

namespace Pretorianie.Tytan.Forms
{
    public partial class DebugViewNewSerialForm : Pretorianie.Tytan.Core.BaseForms.BasePackageForm
    {
        public DebugViewNewSerialForm()
        {
            InitializeComponent();

            // add baud rates:
            cmbBaudRate.Items.Clear();
            foreach (BaudRates b in Enum.GetValues(typeof(BaudRates)))
                cmbBaudRate.Items.Add((int)b);

            // and select the last one:
            if (cmbBaudRate.Items.Count > 0)
                cmbBaudRate.SelectedIndex = cmbBaudRate.Items.Count - 1;

            // add encodings:
            cmbEncodings.Items.Add(Encoding.ASCII);
            cmbEncodings.Items.Add(Encoding.UTF7);
            cmbEncodings.Items.Add(Encoding.UTF8);
            cmbEncodings.Items.Add(Encoding.Unicode);
            cmbEncodings.Items.Add(Encoding.UTF32);
            cmbEncodings.SelectedIndex = 0;
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
            get { return Encoding.ASCII; }
        }

        #endregion
    }
}
