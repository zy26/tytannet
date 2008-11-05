using System;
using System.Collections.Generic;
using System.Diagnostics;
using Pretorianie.Tytan.Core.Tracer;
using System.Reflection;

namespace Pretorianie.Tytan.Core.Tracer
{
    /// <summary>
    /// Class responsible for capturing Tytan.NET execution logs.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Value for <see cref="ConditionalAttribute"/>.
        /// </summary>
        public const string DebugCondition = "DEBUG";

        private static readonly object logSync = new object();
        private static readonly IList<ILogStorage> logStorages = new List<ILogStorage>();

        private const string Format = "{0}|{1}|{2}";
        private const string GenericLogClass = "AppDomain";

        /// <summary>
        /// Message generated when entering method.
        /// </summary>
        public const string StatusEnter = "Enter";
        /// <summary>
        /// Message generated when exiting method.
        /// </summary>
        public const string StatusExit = "Exit";


#if DEBUG
        static Log()
        {
            AppDomain.CurrentDomain.AssemblyLoad += AssemblyLoad;
            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            AppDomain.CurrentDomain.ProcessExit += ProcessExit;
        }

        static void AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            WriteStatus(GenericLogClass, "AssemblyLoad", "Loaded: " + args.LoadedAssembly.FullName);
        }

        static void ProcessExit(object sender, EventArgs e)
        {
            WriteStatus(GenericLogClass, "ProcessExit", "Exiting application. Bye.");
        }

        static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            WriteError(GenericLogClass, "UnhandledException",
                  string.Format("Exception({1}): {0}", e.ExceptionObject, (e.IsTerminating ? "T" : "N")));
        }
#endif

        /// <summary>
        /// Append third-party log filter.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void Append(ILogStorage log)
        {
            if (log == null)
                throw new ArgumentNullException("log", "Can not add log messages to non-existing object");

            lock (logSync)
            {
                logStorages.Add(log);
            }
        }

        #region Error Log

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void WriteError(string className, string methodName, string message)
        {
            string text = string.Format(Format, className, methodName, message);

            lock(logSync)
            {
                foreach(ILogStorage l in logStorages)
                    l.Write(LogImportance.Error, text);
            }
        }

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void WriteError(string className, string methodName, string format, params object[] args)
        {
            WriteError(className, methodName, string.Format(format, args));
        }

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void TraceError(string format, params object[] args)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            MethodBase mb = sf.GetMethod();

            WriteError(mb.DeclaringType.Name, mb.Name, string.Format(format, args));
        }

        /// <summary>
        /// Writes an error message to the log.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void TraceError(Exception e)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            MethodBase mb = sf.GetMethod();

            WriteError(mb.DeclaringType.Name, mb.Name, string.Format("Exception caught: \"{0}\"\r\nStack trace: {1}", e.Message, e.StackTrace));
        }

        #endregion

        #region Warning Log

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void WriteWarning(string className, string methodName, string message)
        {
            string text = string.Format(Format, className, methodName, message);

            lock (logSync)
            {
                foreach (ILogStorage l in logStorages)
                    l.Write(LogImportance.Warning, text);
            }
        }

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void WriteWarning(string className, string methodName, string format, params object[] args)
        {
            WriteWarning(className, methodName, string.Format(format, args));
        }

        /// <summary>
        /// Writes a warning message to the log.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void TraceWarning(string format, params object[] args)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            MethodBase mb = sf.GetMethod();

            WriteWarning(mb.DeclaringType.Name, mb.Name, string.Format(format, args));
        }

        #endregion

        #region Status Log

        /// <summary>
        /// Writes a common message to the log.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void WriteStatus(string className, string methodName, string message)
        {
            string text = string.Format(Format, className, methodName, message);

            lock (logSync)
            {
                foreach (ILogStorage l in logStorages)
                    l.Write(LogImportance.Status, text);
            }
        }

        /// <summary>
        /// Writes a common message to the log.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void WriteStatus(string className, string methodName, string format, params object[] args)
        {
            WriteStatus(className, methodName, string.Format(format, args));
        }

        /// <summary>
        /// Writes a common message to the log.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void TraceStatus(string format, params object[] args)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            MethodBase mb = sf.GetMethod();

            WriteStatus(mb.DeclaringType.Name, mb.Name, string.Format(format, args));
        }

        /// <summary>
        /// Writes a common message to the log, that entered to given function.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void TraceEnter(string format, params object[] args)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            MethodBase mb = sf.GetMethod();

            WriteStatus(mb.DeclaringType.Name, mb.Name, StatusEnter + string.Format(format, args));
        }

        /// <summary>
        /// Writes a common message to the log, that entered to given function.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void TraceEnter()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            MethodBase mb = sf.GetMethod();

            WriteStatus(mb.DeclaringType.Name, mb.Name, StatusEnter);
        }

        /// <summary>
        /// Writes a common message to the log, that exited to given function.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void TraceExit(string format, params object[] args)
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            MethodBase mb = sf.GetMethod();

            WriteStatus(mb.DeclaringType.Name, mb.Name, StatusExit + string.Format(format, args));
        }

        /// <summary>
        /// Writes a common message to the log, that exited to given function.
        /// </summary>
        [Conditional(DebugCondition)]
        public static void TraceExit()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(1);
            MethodBase mb = sf.GetMethod();

            WriteStatus(mb.DeclaringType.Name, mb.Name, StatusExit);
        }

        #endregion

        #region Environment Execution Status

        /// <summary>
        /// Gets the name of class that is currently executed.
        /// </summary>
        public static string ExecutingClassName
        {
            get
            {
                StackTrace st = new StackTrace();
                StackFrame sf = st.GetFrame(1);

                return sf.GetMethod().DeclaringType.Name;
            }
        }

        /// <summary>
        /// Gets the name of currently executed method.
        /// </summary>
        public static string ExecutingMethodName
        {
            get
            {
                StackTrace st = new StackTrace();
                StackFrame sf = st.GetFrame(1);

                return sf.GetMethod().Name;
            }
        }

        #endregion
    }
}
