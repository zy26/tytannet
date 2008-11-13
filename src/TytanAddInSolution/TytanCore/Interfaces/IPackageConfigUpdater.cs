using System;
using Pretorianie.Tytan.Core.Data;

namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// Interface implemented by managers to provide external module info about <see cref="IPackageAction"/> configuration.
    /// </summary>
    public interface IPackageConfigUpdater
    {
        /// <summary>
        /// Sets the configuration description for given action.
        /// </summary>
        void UpdateConfiguration(Type actionType, PersistentStorageData config);

        /// <summary>
        /// Calls an update method related with given button on the configuration GUI.
        /// </summary>
        void UpdateExecute(Type actionType, EventArgs e);
    }
}
