using System.Collections.Generic;

namespace Pretorianie.Tytan.Core.EnvVarView
{
    /// <summary>
    /// Description of environment variables.
    /// </summary>
    public class EnvironmentVariable
    {
        private string name;
        private string value;
        private readonly IList<string> historyValues;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public EnvironmentVariable(string name, string value)
        {
            this.name = name;
            
            historyValues = new List<string>();
            Value = value;
        }

        #region Properties

        /// <summary>
        /// Gets or sets the name of the variable.
        /// </summary>
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Gets or sets the value of the variable.
        /// </summary>
        public string Value
        {
            get { return value; }
            set
            {
                if (!string.IsNullOrEmpty(value) && !historyValues.Contains(value))
                    historyValues.Add(value);

                this.value = value;
            }
        }

        /// <summary>
        /// Gets the history values of the variable.
        /// </summary>
        public IList<string> HistoryValues
        {
            get { return historyValues; }
        }

        #endregion
    }
}
