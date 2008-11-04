using System;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Resources;
using System.Threading;

namespace Pretorianie.Tytan
{
    public static class SharedStringsEx
    {
        #region Internals

        private static readonly ResourceManager rmSharedStrings;
        private static readonly CultureInfo cultureInfo;

        /// <summary>
        /// Registers all the commands and tool-bars.
        /// </summary>
        static SharedStringsEx()
        {
            try
            {
                if (rmSharedStrings == null)
                    rmSharedStrings = new ResourceManager("Pretorianie.Tytan.SharedStrings", Assembly.GetExecutingAssembly());
                if (cultureInfo == null)
                {
                    cultureInfo = Thread.CurrentThread.CurrentUICulture;
                    if(cultureInfo == null)
                        cultureInfo = new CultureInfo("en-US");
                }
            }
            catch (Exception e)
            {
                Trace.TraceError(e.ToString());
                throw;
            }
        }
        
        /// <summary>
        /// Finds the localized menu name from English menu name.
        /// </summary>
        /// <param name="name">Name of the menu</param>
        /// <returns>Localized name</returns>
        /// <remarks>We have to catch all exceptions since GetString method can throw a very extensive
        /// list of exceptions</remarks>
        public static string FindLocalizedMenuName(string name)
        {
            try
            {
                return rmSharedStrings.GetString(string.Concat(cultureInfo.TwoLetterISOLanguageName, name));
            }
            catch
            {
                // tried to find a localized version of the word, but one was not found,
                // so default to the en-US word, which may work for the current culture:
                return name;
            }
        }

        /// <summary>
        /// Reads the value for specific string resource.
        /// </summary>
        public static string GetString(string name)
        {
            try
            {
                return rmSharedStrings.GetString(name);
            }
            catch
            {
                return null;
            }
        }

        #endregion

        #region Values

        public static string ToolWindowTitle_DebugView
        {
            get { return GetString("ToolWindowTitle_DebugView"); }
        }

        public static string ToolWindowTitle_TypeFinder
        {
            get { return GetString("ToolWindowTitle_TypeFinder"); }
        }

        public static string ToolWindowTitle_RegistryView
        {
            get { return GetString("ToolWindowTitle_RegistryView"); }
        }

        public static string ToolWindowTitle_EnvironmentVars
        {
            get { return GetString("ToolWindowTitle_EnvironmentVars"); }
        }

        public static string ToolWindowTitle_NativeImagePreview
        {
            get { return GetString("ToolWindowTitle_NativeImagePreview"); }
        }

        public static string UndoContext_ExtractPropertyRefactor
        {
            get { return GetString("UndoContext_ExtractPropertyRefactor"); }
        }

        public static string UndoContext_InitConstructorRefactor
        {
            get { return GetString("UndoContext_InitConstructorRefactor"); }
        }

        public static string UndoContext_AssignReorderRefactor
        {
            get { return GetString("UndoContext_AssignReorderRefactor"); }
        }

        public static string UndoContext_MultiRenameRefactor
        {
            get { return GetString("UndoContext_MultiRenameRefactor"); }
        }

        #endregion

    }
}
