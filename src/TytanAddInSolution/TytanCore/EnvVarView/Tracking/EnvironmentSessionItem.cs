using System;

namespace Pretorianie.Tytan.Core.EnvVarView.Tracking
{
    /// <summary>
    /// Item of change session for environment variables.
    /// </summary>
    public class EnvironmentSessionItem
    {
        /// <summary>
        /// Init constructor.
        /// </summary>
        public EnvironmentSessionItem(EnvironmentVariableTarget target, string name, string value)
            : this(target, name, value, null)
        {
        }

        /// <summary>
        /// Init constructor.
        /// </summary>
        public EnvironmentSessionItem(EnvironmentVariableTarget target, string name, string value, string primaryValue)
        {
            Target = target;
            Name = name;
            Value = value;
            PrimaryValue = primaryValue;
        }

        #region Properties

        /// <summary>
        /// Gets the target (scope) of the original 
        /// </summary>
        public EnvironmentVariableTarget Target
        { get; protected set; }

        /// <summary>
        /// Gets the name of the changed environment variable.
        /// </summary>
        public string Name
        { get; protected set; }

        /// <summary>
        /// Gets the current value of the environment variable.
        /// </summary>
        public string Value
        { get; set; }

        /// <summary>
        /// Gets the original value assigned to the environment variable.
        /// </summary>
        public string PrimaryValue
        { get; private set; }

        /// <summary>
        /// Gets the name of the action performed over that environment variable.
        /// </summary>
        public string Action
        {
            get
            {
                // evaluate the action performed over an environment variable:
                if (string.IsNullOrEmpty(PrimaryValue))
                    return "Added";

                if (string.IsNullOrEmpty(Value))
                    return "Deleted";

                return "Modified";
            }
        }

        #endregion

        /// <summary>
        /// Reverts changes introduced by this environment variable.
        /// </summary>
        public void Revert()
        {
            Environment.SetEnvironmentVariable(Name, PrimaryValue, Target);
        }
    }
}
