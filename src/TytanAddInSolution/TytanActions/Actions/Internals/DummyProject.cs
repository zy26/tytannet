using EnvDTE;

namespace Pretorianie.Tytan.Actions.Internals
{
    /// <summary>
    /// Implementation of 
    /// </summary>
    internal class DummyProject : Project
    {
        private string name;

        public DummyProject(string name)
        {
            this.name = name;
        }

        #region Project Implementation

        public void SaveAs(string newFileName)
        {
        }

        public void Save(string fileName)
        {
        }

        public void Delete()
        {
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string FileName
        {
            get { return string.Empty; }
        }

        public bool IsDirty
        {
            get { return false; }
            set { }
        }

        public Projects Collection
        {
            get { return null; }
        }

        public DTE DTE
        {
            get { return null; }
        }

        public string Kind
        {
            get { return string.Empty; }
        }

        public ProjectItems ProjectItems
        {
            get { return null; }
        }

        public Properties Properties
        {
            get { return null; }
        }

        public string UniqueName
        {
            get { return string.Empty; }
        }

        public object Object
        {
            get { return null; }
        }

        public object ExtenderNames
        {
            get { return null; }
        }

        public string ExtenderCATID
        {
            get { return string.Empty; }
        }

        public string FullName
        {
            get { return string.Empty; }
        }

        public bool Saved
        {
            get { return true; }
            set { }
        }

        public ConfigurationManager ConfigurationManager
        {
            get { return null; }
        }

        public Globals Globals
        {
            get { return null; }
        }

        public ProjectItem ParentProjectItem
        {
            get { return null; }
        }

        public CodeModel CodeModel
        {
            get { return null; }
        }

        public object get_Extender(string ExtenderName)
        {
            return null;
        }

        #endregion
    }
}
