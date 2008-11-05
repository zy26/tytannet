using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using System.Text;

namespace Pretorianie.Tytan.Forms
{
    public partial class SystemComVisualizerForm : Form
    {
        private const int InterfaceIconA = 0;
        private const int InterfaceIconB = 1;
        private const int MethodIconA = 7;
        private const int MethodIconB = 7;
        private const int BaseTypeIconA = 3;
        private const int BaseTypeIconB = 4;
        private const int BaseInterfaceA = 6;
        private const int BaseInterfaceB = 6;
        private const int BaseClassIconA = 5;
        private const int BaseClassIconB = 5;

        public SystemComVisualizerForm()
        {
            InitializeComponent();
        }

        public SystemComVisualizerForm(IList<Type> types)
            : this()
        {
            if (types != null)
            {
                typesTree.BeginUpdate();

                foreach (Type t in types)
                {
                    TreeNode n = new TreeNode(t.FullName, InterfaceIconA, InterfaceIconB);
                    n.Tag = t;

                    // serialize info:
                    n.Nodes.AddRange(ToTreeNode(t));

                    // store the type on the screen:
                    typesTree.Nodes.Add(n);
                    n.Expand();
                }

                typesTree.EndUpdate();
            }
        }

        private static TreeNode[] ToTreeNode(Type t)
        {
            List<TreeNode> result = new List<TreeNode>();

            // add base types:
            TreeNode bt = new TreeNode("Base Types", BaseTypeIconA, BaseTypeIconB);
            Type[] baseInterfaces = t.GetInterfaces();

            TreeNode parent = bt;
            TreeNode node;
            Type x = t;
            while (x.BaseType != null)
            {
                parent.Nodes.Add(parent = new TreeNode(x.BaseType.Name, BaseClassIconA, BaseClassIconB));
                parent.Tag = x.BaseType;
                x = x.BaseType;
            }

            if (baseInterfaces != null)
                foreach (Type i in baseInterfaces)
                {
                    bt.Nodes.Add(node = new TreeNode(i.Name, BaseInterfaceA, BaseInterfaceB));
                    node.Tag = i;
                }

            if(bt.Nodes.Count > 0)
                result.Add(bt);

            // add methods:
            foreach (MethodInfo m in t.GetMethods())
            {
                TreeNode n =
                    new TreeNode(string.Format("{0} {1} ({2})", m.ReturnType.Name, m.Name, ParametersToString(m)), MethodIconA, MethodIconB);
                result.Add(n);
            }

            return result.ToArray();
        }

        private static string ParametersToString(MethodInfo method)
        {
            StringBuilder result = new StringBuilder();
            int count = 0;

            foreach (ParameterInfo p in method.GetParameters())
            {
                if (count > 0)
                    result.Append(", ");
                AppendParameter(result, p);
                count++;
            }

            return result.ToString();
        }

        private static void AppendParameter(StringBuilder output, ParameterInfo p)
        {
            if (p.IsOut)
                output.Append("ref ");
            else
                if (p.IsRetval)
                    output.Append("out ");

            output.Append(p.ParameterType.Name);
            output.Append(' ');
            output.Append(p.Name);
        }

        private void typesTree_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode parent = e.Node;
            string typeName = string.Empty;

            if (parent != null)
            {
                while (parent.Parent != null && parent.Tag == null)
                    parent = parent.Parent;

                if(parent.Tag is Type)
                    typeName = ((Type)parent.Tag).AssemblyQualifiedName;
            }

            // display the full name of given type:
            txtTypeName.Text = typeName;
        }
    }
}