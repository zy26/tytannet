using System;
using System.Collections.Generic;
using System.Collections;

namespace Pretorianie.Tytan.Core.EnvVarView
{
    /// <summary>
    /// Class that manages the environment variables.
    /// </summary>
    public class EnvironmentVariables : IEnumerable<EnvironmentVariable>
    {
        private IList<EnvironmentVariable> vars = new List<EnvironmentVariable>();
        private readonly EnvironmentVariableTarget target;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public EnvironmentVariables(EnvironmentVariableTarget target)
        {
            this.target = target;
            ReadVariables(target);
        }

        #region Properties

        /// <summary>
        /// Gets the number of stored variables.
        /// </summary>
        public int Count
        {
            get { return vars.Count; }
        }

        /// <summary>
        /// Gets the list of the variables.
        /// </summary>
        public IList<EnvironmentVariable> Items
        {
            get { return vars; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fills the internal collection with the values read from the environment.
        /// </summary>
        public void ReadVariables(EnvironmentVariableTarget varTarget)
        {
            // read all variables:
            vars.Clear();
            foreach (DictionaryEntry e in Environment.GetEnvironmentVariables(varTarget))
                vars.Add(new EnvironmentVariable((string)e.Key, (string)e.Value));
        }

        /// <summary>
        /// Reads the variables once again with history persistance.
        /// </summary>
        public void Refresh()
        {
            IList<EnvironmentVariable> oldVars = vars;
            EnvironmentVariable x;

            vars = new List<EnvironmentVariable>(oldVars.Count);

            // read once again and look if specified variable existed:
            foreach (DictionaryEntry e in Environment.GetEnvironmentVariables(target))
            {
                x = Find(oldVars, (string)e.Key);
                if (x != null)
                {
                    x.Value = (string)e.Value;
                    vars.Add(x);
                }
                else
                    vars.Add(new EnvironmentVariable((string)e.Key, (string)e.Value));
            }
        }

        private static EnvironmentVariable Find(IList<EnvironmentVariable> vars, string name)
        {
            if ( vars != null )
                foreach (EnvironmentVariable v in vars)
                {
                    if (string.Compare(v.Name, name, true) == 0)
                        return v;
                }

            return null;
        }

        /// <summary>
        /// Removes the variable with specified name.
        /// </summary>
        public void Remove(string name)
        {
            EnvironmentVariable v = Find(vars, name);

            if (v != null)
            {
                vars.Remove(v);
                Environment.SetEnvironmentVariable(name, null, target);
            }
        }

        /// <summary>
        /// Updates the value of specified variable.
        /// </summary>
        public void SetVariable(string name, string value)
        {
            Environment.SetEnvironmentVariable(name, value, target);
            if (target != EnvironmentVariableTarget.Process && !string.IsNullOrEmpty(value))
                Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Process);
        }

        #endregion

        #region IEnumerable<EnvironmentVariable> Members

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        public IEnumerator<EnvironmentVariable> GetEnumerator()
        {
            return vars.GetEnumerator ();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return vars.GetEnumerator();
        }

        #endregion
    }
}
