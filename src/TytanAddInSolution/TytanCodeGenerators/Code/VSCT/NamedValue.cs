namespace Pretorianie.Tytan.Code.VSCT
{
    /// <summary>
    /// Type storing the value of its name.
    /// </summary>
    internal class NamedValue
    {
        private readonly string name;
        private string supporter;
        private readonly string value;
        private readonly NamedValue parent;

        public NamedValue(string name, string value)
        {
            this.name = name;
            this.value = value;
        }

        public NamedValue(string name, string value, NamedValue parent)
        {
            this.name = name;
            this.parent = parent;
            this.value = value;
        }

        public string Name
        {
            get { return name; }
        }

        public string Supporter
        {
            get { return supporter; }
            set { supporter = value; }
        }

        public string Value
        {
            get { return value; }
        }

        public NamedValue Parent
        {
            get { return parent; }
        }
    }
}
