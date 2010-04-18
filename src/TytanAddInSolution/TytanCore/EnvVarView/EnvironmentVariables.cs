using System;
using System.Collections.Generic;
using System.Collections;
using Pretorianie.Tytan.Core.EnvVarView.Tracking;

namespace Pretorianie.Tytan.Core.EnvVarView
{
    /// <summary>
    /// Class that manages the environment variables.
    /// </summary>
    public class EnvironmentVariables : IEnumerable<EnvironmentVariable>
    {
        private IList<EnvironmentVariable> _vars = new List<EnvironmentVariable>();
        private readonly EnvironmentVariableTarget _target;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public EnvironmentVariables(EnvironmentVariableTarget target)
        {
            _target = target;
            ReadVariables(target);
            LoadHistory();
        }

        #region Properties

        /// <summary>
        /// Gets the number of stored variables.
        /// </summary>
        public int Count
        {
            get { return _vars.Count; }
        }

        /// <summary>
        /// Gets the list of the variables.
        /// </summary>
        public IList<EnvironmentVariable> Items
        {
            get { return _vars; }
        }

        /// <summary>
        /// Gets the target of the current environment variable group.
        /// </summary>
        public EnvironmentVariableTarget Target
        {
            get { return _target; }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Fills the internal collection with the values read from the environment.
        /// </summary>
        public void ReadVariables(EnvironmentVariableTarget varTarget)
        {
            // read all variables:
            _vars.Clear();
            foreach (DictionaryEntry e in Environment.GetEnvironmentVariables(varTarget))
                _vars.Add(new EnvironmentVariable((string)e.Key, (string)e.Value));

            // and load the history values from persistent storage:
        }

        /// <summary>
        /// Reads the variables once again with history persistence.
        /// </summary>
        public void Refresh()
        {
            IList<EnvironmentVariable> oldVars = _vars;
            EnvironmentVariable x;

            _vars = new List<EnvironmentVariable>(oldVars.Count);

            // read once again and look if specified variable existed:
            foreach (DictionaryEntry e in Environment.GetEnvironmentVariables(_target))
            {
                x = Find(oldVars, (string)e.Key);
                if (x != null)
                {
                    x.Value = (string)e.Value;
                    _vars.Add(x);
                }
                else
                    _vars.Add(new EnvironmentVariable((string)e.Key, (string)e.Value));
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
            EnvironmentVariable v = Find(_vars, name);

            if (v != null)
            {
                _vars.Remove(v);
                Environment.SetEnvironmentVariable(name, null, _target);
            }
        }

        /// <summary>
        /// Updates the value of specified variable.
        /// </summary>
        public void SetVariable(string name, string value)
        {
            Environment.SetEnvironmentVariable(name, value, _target);
            if (_target != EnvironmentVariableTarget.Process && !string.IsNullOrEmpty(value))
                Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Process);

            SaveHistory();
        }

        /// <summary>
        /// Gets the value of particular variable or <c>null</c> if doesn't exist.
        /// </summary>
        public EnvironmentVariable GetVariable(string name)
        {
            return Find(_vars, name);
        }

        /// <summary>
        /// Loads history values of changed variables from persistent storage.
        /// </summary>
        public void LoadHistory()
        {
            Dictionary<string, IList<string>> history = EnvironmentSessionProvider.LoadHistory(Target);
            EnvironmentVariable x;

            if (history != null)
            {
                foreach(KeyValuePair<string, IList<string>> item in history)
                {
                    x = Find(_vars, item.Key);

                    if (x != null)
                        x.ApplyHistory(item.Value);
                }
            }
        }

        /// <summary>
        /// Saves current history values into persistent storage.
        /// </summary>
        public void SaveHistory()
        {
            EnvironmentSessionProvider.SaveHistory(Target, _vars);
        }

        /// <summary>
        /// Clears the history values and removes from persistent storage.
        /// </summary>
        public void ClearHistory()
        {
            EnvironmentSessionProvider.ClearHistory(Target);

            foreach (EnvironmentVariable v in _vars)
                v.ClearHistory();
        }

        #endregion

        #region IEnumerable<EnvironmentVariable> Members

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        public IEnumerator<EnvironmentVariable> GetEnumerator()
        {
            return _vars.GetEnumerator ();
        }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _vars.GetEnumerator();
        }

        #endregion
    }
}
