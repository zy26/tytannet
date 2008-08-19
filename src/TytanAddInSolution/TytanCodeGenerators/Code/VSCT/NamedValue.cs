namespace Pretorianie.Tytan.Code.VSCT
{
    /// <summary>
    /// Type storing the value of its name.
    /// </summary>
    class NamedValue
    {
        private readonly string name;
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
