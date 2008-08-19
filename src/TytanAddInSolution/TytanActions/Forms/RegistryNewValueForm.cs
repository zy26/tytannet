using System;
using System.Globalization;
using System.Text;
using Microsoft.Win32;
using Pretorianie.Tytan.Core.BaseForms;
using System.Drawing;

namespace Pretorianie.Tytan.Forms
{
    public partial class RegistryNewValueForm : BasePackageForm
    {
        private Font standardFont;
        private Font monoFont;


        public RegistryNewValueForm()
        {
            InitializeComponent();

            standardFont = txtValue.Font;
            monoFont = new Font("Courier New", 12, standardFont.Unit);
        }

        /// <summary>
        /// Initialize the whole interface.
        /// </summary>
        public void SetUI(string title, string name, object value, RegistryValueKind defaultType, params RegistryValueKind[] types)
        {
            Text = title;

            // populate the list of types:
            cmbType.Items.Clear();
            if (types != null)
                foreach (RegistryValueKind k in types)
                    if (!cmbType.Items.Contains(k))
                        cmbType.Items.Add(k);

            // check if default value is on the list:
            if (!cmbType.Items.Contains(defaultType))
                cmbType.Items.Add(defaultType);

            cmbType.SelectedItem = defaultType;
            ActiveControl = txtName;

            RegistryValueName = name;
            SetRegistryValue(value, defaultType);
        }

        public void SetRegistryValue(object value, RegistryValueKind type)
        {
            string text = null;
            string[] strings;
            byte[] data;
            StringBuilder s;

            switch (type)
            {
                case RegistryValueKind.DWord:
                case RegistryValueKind.QWord:
                case RegistryValueKind.ExpandString:
                case RegistryValueKind.String:
                    text = value.ToString();
                    break;
                case RegistryValueKind.MultiString:
                    s = new StringBuilder();
                    strings = (string[])value;
                    if (strings != null)
                        foreach (string m in strings)
                            s.AppendLine(m);
                    text = s.ToString();
                    break;
                case RegistryValueKind.Binary:
                    s = new StringBuilder();
                    data = (byte[])value;
                    int counter = 0;
                    if (value != null)
                        for (int i = 0; i < data.Length; i++)
                        {
                            s.AppendFormat("0x{0:X} ", data[i]);
                            counter++;
                            if ((counter % 8) == 0)
                                s.AppendLine();
                        }
                    text = s.ToString();
                    break;
            }

            txtValue.Text = text;
        }

        private object GetRegistryValue(RegistryValueKind type)
        {
            switch (type)
            {
                case RegistryValueKind.DWord:
                case RegistryValueKind.QWord:
                    return GetUInt32Value(txtValue.Text);
                case RegistryValueKind.MultiString:
                    return txtValue.Text.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                case RegistryValueKind.Binary:
                    return GetBinaryValue(txtValue.Text.Split(new char[] { '\r', '\n', ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));

                default:
                    return txtValue.Text;
            }
        }

        private byte[] GetBinaryValue(string[] data)
        {
            if (data != null && data.Length > 0)
            {
                byte[] result = new byte[data.Length];

                for (int i = 0; i < data.Length; i++)
                    result[i] = GetByteValue(data[i]);

                return result;
            }
            else
                return null;
        }

        private object GetUInt32Value(string txt)
        {
            UInt64 result;

            if (UInt64.TryParse(txt, out result))
                return result;
            else
                if (txt != null && txt.Length > 2 && UInt64.TryParse(txt.Substring(2), NumberStyles.HexNumber, null, out result))
                    return result;
                else
                    return txt;
        }

        private byte GetByteValue(string txt)
        {
            byte result;

            if (byte.TryParse(txt, out result))
                return result;
            else
                if (txt != null && txt.Length > 2 && byte.TryParse(txt.Substring(2), NumberStyles.HexNumber, null, out result))
                    return result;
                else
                    return 0;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the name of the value.
        /// </summary>
        public string RegistryValueName
        {
            get { return txtName.Text; }
            set { txtName.Text = value; txtName.SelectionLength = txtName.Text.Length; txtName.SelectionStart = 0; }
        }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public object RegistryValue
        {
            get { return GetRegistryValue(RegistryValueType); }
        }

        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        public RegistryValueKind RegistryValueType
        {
            get { return (RegistryValueKind)cmbType.SelectedItem; }
        }

        #endregion

        private void bttWordWrap_Click(object sender, EventArgs e)
        {
            txtValue.WordWrap = !txtValue.WordWrap;
        }

        private void bttToUpper_Click(object sender, EventArgs e)
        {
            txtValue.Text = txtValue.Text.ToUpper();
        }

        private void bttToLower_Click(object sender, EventArgs e)
        {
            txtValue.Text = txtValue.Text.ToLower();
        }

        private void bttToMonoFont_Click(object sender, EventArgs e)
        {
            if ( txtValue.Font == standardFont)
                txtValue.Font = monoFont;
            else
                txtValue.Font = standardFont;
        }
    }
}