using System;
using System.Text;
using System.Windows.Forms;
using System.Xml;

namespace Pretorianie.Tytan.Forms
{
    public partial class XmlVisualizerForm : Form
    {
        private const int DocumentImageA = 0;
        private const int DocumentImageB = 0;
        private const int NodeImageA = 1;
        private const int NodeImageB = 2;
        private const int NodeAttributeImageA = 3;
        private const int NodeAttributeImageB = 4;
        private const int NodeNamespaceImageA = 5;
        private const int NodeNamespaceImageB = 6;
        private const int NodeValueImageA = 7;
        private const int NodeValueImageB = 7;

        private ScreenViews currentView;

        private enum ScreenViews
        {
            TreeView,
            Text,
            GraphicalPreview,
            ErrorMessage
        }

        public XmlVisualizerForm()
        {
            InitializeComponent();

            Show(ScreenViews.ErrorMessage);
        }

        public XmlVisualizerForm(string data)
        {
            XmlDocument xml;
            string error = null;

            InitializeComponent();

            try
            {
                xml = new XmlDocument();
                xml.LoadXml(data);
            }
            catch (Exception ex)
            {
                xml = null;
                error = ex.Message;
            }

            Text = string.Format("XML document (length: {0})", (string.IsNullOrEmpty(data) ? 0 : data.Length));
            Show(ScreenViews.ErrorMessage);
            UpdateView(xml, data, error);
        }

        private void Show(ScreenViews screen)
        {
            treeXml.Visible = false;
            textXml.Visible = false;
            labelError.Visible = false;
            currentView = screen;

            switch (screen)
            {
                case ScreenViews.TreeView:
                    treeXml.Visible = true;
                    break;
                case ScreenViews.Text:
                    textXml.Visible = true;
                    break;
                case ScreenViews.ErrorMessage:
                    labelError.Visible = true;
                    break;
            }
        }

        private void ShowExt(ScreenViews screen)
        {
            if (screen == currentView && !string.IsNullOrEmpty(labelError.Text))
                Show(ScreenViews.ErrorMessage);
            else Show(screen);
        }

        private static TreeNode BuildXmlNode(XmlNode x, int imageA, int imageB)
        {
            TreeNode n = new TreeNode(x.Name, imageA, imageB);
            TreeNode bs = (string.IsNullOrEmpty(x.BaseURI)
                               ? null
                               : new TreeNode("base URI: " + x.BaseURI, NodeNamespaceImageA, NodeNamespaceImageB));
            TreeNode ns = (string.IsNullOrEmpty(x.NamespaceURI)
                               ? null
                               : new TreeNode("namespace: " + x.NamespaceURI, NodeNamespaceImageA, NodeNamespaceImageB));
            TreeNode v = (string.IsNullOrEmpty(x.Value) ? null : new TreeNode("value", NodeValueImageA, NodeValueImageB));
            TreeNode value = (string.IsNullOrEmpty(x.Value)
                                  ? null
                                  : new TreeNode(x.Value, NodeValueImageA, NodeValueImageB));

            if (bs != null)
                n.Nodes.Add(bs);
            if (ns != null)
                n.Nodes.Add(ns);

            // serialize attributes:
            XmlAttributeCollection attrs = x.Attributes;

            if(attrs != null)
                foreach (XmlAttribute a in attrs)
                {
                    TreeNode attr = (string.IsNullOrEmpty(a.Prefix)
                                         ?
                                             new TreeNode(string.Format("{0} = \"{1}\"", a.Name, a.Value),
                                                          NodeAttributeImageA, NodeAttributeImageB)
                                         :
                                             new TreeNode(
                                                 string.Format("{0}:{1} = \"{2}\"", a.Prefix, a.Name, a.Value),
                                                 NodeAttributeImageA,
                                                 NodeAttributeImageB));
                    n.Nodes.Add(attr);
                }

            if (value != null && v != null)
                v.Nodes.Add(value);
            if (v != null)
                n.Nodes.Add(v);

            return n;
        }

        private static void BuildXmlTree(TreeNode parent, XmlNodeList nodes)
        {
            if (parent != null)
            {
                foreach (XmlNode x in nodes)
                {
                    if (x.NodeType == XmlNodeType.Element)
                    {
                        // serialize current node:
                        TreeNode n = BuildXmlNode(x, NodeImageA, NodeImageB);

                        // if it is valid, go throu all the childs and do the same:
                        if (n != null)
                        {
                            BuildXmlTree(n, x.ChildNodes);
                            parent.Nodes.Add(n);
                        }
                    }
                }
            }
        }

        private void UpdateView(XmlDocument xml, string data, string error)
        {
            // store XML tree:
            if (xml != null)
            {
                TreeNode mainNode;

                if (xml.DocumentElement == null || string.IsNullOrEmpty(xml.DocumentElement.Name))
                    mainNode = new TreeNode(SharedResources.Error_EmptyXmlDocument, DocumentImageA, DocumentImageB);
                else
                {
                    mainNode = BuildXmlNode(xml.DocumentElement, DocumentImageA, DocumentImageB);
                    
                    // serialize all XML document into TreeView:
                    BuildXmlTree(mainNode, xml.DocumentElement.ChildNodes);
                }

                treeXml.Nodes.Clear();
                treeXml.Nodes.Add(mainNode);
                mainNode.ExpandAll();
            }

            // store raw-text data:
            if (xml == null)
                textXml.Text = data;
            else
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                StringBuilder result = new StringBuilder();
           
                settings.Indent = true;
                XmlWriter output = XmlWriter.Create(result, settings);

                xml.WriteTo(output);
                output.Flush();
                textXml.Text = result.ToString();
            }

            // store error:
            labelError.Text = error;

            if (string.IsNullOrEmpty(error))
                Show(ScreenViews.TreeView);
            else Show(ScreenViews.ErrorMessage);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            ShowExt(ScreenViews.TreeView);
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ShowExt(ScreenViews.Text);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            ShowExt(ScreenViews.GraphicalPreview);
        }
    }
}