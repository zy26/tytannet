using System;
using System.Collections.Generic;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Core.CustomPackage
{
    /// <summary>
    /// Class that manages the collection of custom actions.
    /// </summary>
    public class PackageActionCollection : IDisposable
    {
        private readonly Dictionary<int, IPackageAction> data = new Dictionary<int, IPackageAction>();

        /// <summary>
        /// Store new action for group command execution and management.
        /// </summary>
        public void Add(IPackageAction action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            if (data.ContainsKey(action.ID))
                data[action.ID] = action;
            else
                data.Add(action.ID, action);
        }

        /// <summary>
        /// Invoke initialization for all stored actions.
        /// </summary>
        public void Initialize(IPackageEnvironment env, System.ComponentModel.Design.IMenuCommandService mcs, IMenuCreator mc)
        {
            foreach (IPackageAction a in data.Values)
                a.Initialize(env, mcs, mc);
        }

        /// <summary>
        /// Releases action's memory and stores their states.
        /// </summary>
        public void Dispose()
        {
            foreach (IPackageAction a in data.Values)
                a.Dispose();
        }

        /// <summary>
        /// Indexer.
        /// </summary>
        public IPackageAction this[int id]
        {
            get
            {
                IPackageAction result;

                if (data.TryGetValue(id, out result))
                    return result;
                return null;
            }
        }
    }
}