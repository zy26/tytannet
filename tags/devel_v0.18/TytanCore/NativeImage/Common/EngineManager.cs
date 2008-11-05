using System;
using System.Collections.Generic;

namespace Pretorianie.Tytan.Core.NativeImage.Common
{
    /// <summary>
    /// Manager class that stores the list of all available engines for native image processing.
    /// </summary>
    public class EngineManager
    {
        private readonly IList<IEngine> engines = new List<IEngine>();
        private Dictionary<string, IList<IEngine>> extEngines;

        /// <summary>
        /// Registers new processor engine for native images.
        /// </summary>
        public void Register(IEngine engine)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");

            if (!engines.Contains(engine))
            {
                engines.Add(engine);
                extEngines = null;
            }
        }

        /// <summary>
        /// Removes specified engine from the list.
        /// </summary>
        public void Unregister(IEngine engine)
        {
            if (engine == null)
                throw new ArgumentNullException("engine");

            if(engines.Remove(engine))
                extEngines = null;
        }

        /// <summary>
        /// Gets the collection of native processor engines that are able to work with given extension.
        /// </summary>
        public IList<IEngine> GetMatching(string extension)
        {
            List<IEngine> result = new List<IEngine>();

            foreach(IEngine e in engines)
            {
                IList<EngineFileExtension> exts = e.FileExtensions;

                // validate extension:
                if(exts != null)
                {
                    foreach(EngineFileExtension ext in exts)
                        if(string.Compare(ext.Extension, extension, StringComparison.CurrentCultureIgnoreCase)==0)
                        {
                            result.Add(e);
                            break;
                        }
                }
            }

            return result.Count == 0 ? null : result;
        }

        #region Properties

        /// <summary>
        /// Gets the list of all engines.
        /// </summary>
        public IList<IEngine> Engines
        {
            get { return engines; }
        }

        /// <summary>
        /// Gets the list of engines sorted by the extensions.
        /// </summary>
        public Dictionary<string, IList<IEngine>> EnginesByExtension
        {
            get
            {
                if (extEngines == null)
                {
                    extEngines = new Dictionary<string, IList<IEngine>>();

                    foreach(IEngine e in engines)
                    {
                        IList<EngineFileExtension> exts = e.FileExtensions;

                        if(exts != null)
                        {
                            foreach(EngineFileExtension ext in exts)
                            {
                                IList<IEngine> m;

                                // add new list of engines or add to the list:
                                if(extEngines.TryGetValue(ext.Extension, out m))
                                    m.Add(e);
                                else
                                {
                                    m = new List<IEngine>();
                                    m.Add(e);
                                    extEngines.Add(ext.Extension, m);
                                }
                            }
                        }
                    }
                }

                return extEngines;
            }
            
        }

        #endregion
    }
}
