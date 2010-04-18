using System.Collections.Generic;

namespace Pretorianie.Tytan.Core.EnvVarView
{
    /// <summary>
    /// Description of environment variables.
    /// </summary>
    public class EnvironmentVariable
    {
        private string _name;
        private string _value;
        private readonly IList<string> _historyValues;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public EnvironmentVariable(string name, string value)
        {
            _name = name;

            _historyValues = new List<string>();
            Value = value;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the name of the variable.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        /// Gets or sets the value of the variable.
        /// </summary>
        public string Value
        {
            get { return _value; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !_historyValues.Contains(value))
                    _historyValues.Add(value);

                this._value = value;
            }
        }

        /// <summary>
        /// Gets the history values of the variable.
        /// </summary>
        public IList<string> HistoryValues
        {
            get { return _historyValues; }
        }

        #endregion

        /// <summary>
        /// Appends new history values.
        /// </summary>
        public void ApplyHistory(IList<string> entries)
        {
            if (entries != null)
            {
                foreach (string i in entries)
                    if (!string.IsNullOrEmpty(i) && !_historyValues.Contains(i))
                        _historyValues.Add(i);
            }
        }

        /// <summary>
        /// Removes all history values.
        /// </summary>
        public void ClearHistory()
        {
            _historyValues.Clear();
            _historyValues.Add(_value);
        }
    }
}
