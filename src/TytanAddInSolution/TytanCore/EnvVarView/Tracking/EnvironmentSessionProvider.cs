using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Pretorianie.Tytan.Core.EnvVarView.Tracking
{
    /// <summary>
    /// Factory class for environment variable sessions.
    /// </summary>
    public static class EnvironmentSessionProvider
    {
        private const string SessionExtension = ".session.txt";
        private const string HistoryExtension = ".txt";
        private const string ConfigDirectory = @"\TytanNET\Environment\";
        private static string environmentStoragePath;

        static EnvironmentSessionProvider()
        {
            // by default set the path with stored sessions and other config files
            // to 'My Documents' folder:
            environmentStoragePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                     + ConfigDirectory;
        }

        private static string PreparePath(string givenPath)
        {
            string path = Path.GetDirectoryName(givenPath);

            if (path == null)
                return null;

            if (path.StartsWith("file:\\\\"))
                path = path.Substring(7);
            if (path.StartsWith("file:\\"))
                path = path.Substring(6);

            if (path != null && path[0] != '\\' && !(char.ToUpper(path[0]) >= 'A' && char.ToUpper(path[0]) <= 'Z' && path[1] == ':'))
                path = @"\\" + path;

            // return path to use:
            return path + @"\";
        }

        /// <summary>
        /// Updates the path, where files with sessions are stored.
        /// </summary>
        public static void UpdateEnvironmentStoragePath(string path)
        {
            environmentStoragePath = PreparePath(path);
        }

        /// <summary>
        /// Converts any string into a valid name of the file.
        /// </summary>
        private static string PrepareFileName(string name)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");

            char[] invalidChars = Path.GetInvalidFileNameChars();
            string fileName = name;

            for (int i = 0; i < invalidChars.Length; i++)
            {
                if (fileName.IndexOf(invalidChars[i]) >= 0)
                    fileName = fileName.Replace(invalidChars[i], '_');
            }

            return fileName;
        }

        /// <summary>
        /// Loads previously stored sessions from persistent storage.
        /// </summary>
        public static IEnumerable<EnvironmentSession> LoadSessions()
        {
            IList<EnvironmentSession> sessions = new List<EnvironmentSession>();
            string[] fileNames;

            try
            {
                Directory.CreateDirectory(environmentStoragePath);
                fileNames = Directory.GetFiles(environmentStoragePath, "*" + SessionExtension, SearchOption.AllDirectories);
            }
            catch (Exception ex)
            {
                fileNames = null;
                Trace.WriteLine("Unable to open directory: '" + environmentStoragePath + "'.");
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }

            // load data from each session file:
            if (fileNames != null && fileNames.Length > 0)
            {
                foreach(string file in fileNames)
                {
                    try
                    {
                        EnvironmentSession session = null;

                        using (StreamReader reader = File.OpenText(file))
                        {
                            if (session == null)
                                session = new EnvironmentSession();
                            session.LoadFrom(reader, file);

                            if (session.Count > 0)
                            {
                                if (string.IsNullOrEmpty(session.Name))
                                {
                                    string name = Path.GetFileName(file);
                                    session.Name = name.Substring(0, name.Length - SessionExtension.Length);
                                }
                                sessions.Add(session);
                                session = null;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("Unable to load session from file: '" + file + "'.");
                        Trace.WriteLine(ex.Message);
                        Trace.WriteLine(ex.StackTrace);
                    }
                }
            }

            return sessions;
        }

        /// <summary>
        /// Writes given environment session variables changes into a persistent storage.
        /// </summary>
        public static void SaveSession(string name, IEnumerable<EnvironmentSessionItem> items)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            if (items == null)
                throw new ArgumentNullException("items");

            SaveSession(name, new EnvironmentSession(name, items));
        }

        /// <summary>
        /// Writes given environment session variables changes into a persistent storage.
        /// </summary>
        public static void SaveSession (string fileName, EnvironmentSession session)
        {
            using (StreamWriter outputFile = new StreamWriter(
                environmentStoragePath + PrepareFileName(fileName) + SessionExtension, false, Encoding.UTF8))
            {
                session.WriteTo(outputFile);
            }
        }

        /// <summary>
        /// Gets the name of the history file associated with given target.
        /// </summary>
        private static string GetHistoryFileName (EnvironmentVariableTarget target)
        {
            return environmentStoragePath + "History." + target + HistoryExtension;
        }

        /// <summary>
        /// Removes the history file associated with given type of variables.
        /// </summary>
        public static void ClearHistory(EnvironmentVariableTarget target)
        {
            try
            {
                File.Delete(GetHistoryFileName(target));
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Saves the history values of variables associated with given type.
        /// </summary>
        public static void SaveHistory(EnvironmentVariableTarget target, IList<EnvironmentVariable> variables)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(GetHistoryFileName(target), false, Encoding.UTF8))
                {
                    if (variables != null)
                    {
                        foreach (EnvironmentVariable v in variables)
                        {
                            if (v.HistoryValues.Count > 1)
                            {
                                foreach (string h in v.HistoryValues)
                                    outputFile.WriteLine("{0}={1}", v.Name, h);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
        }

        /// <summary>
        /// Loads history values for variables of given 
        /// </summary>
        public static Dictionary<string, IList<string>> LoadHistory(EnvironmentVariableTarget target)
        {
            Dictionary<string, IList<string>> data = null;

            try
            {
                using (StreamReader reader = File.OpenText(GetHistoryFileName(target)))
                {
                    data = new Dictionary<string, IList<string>>();
                    string line;
                    string name;
                    string value;
                    int index;
                    IList<string> items;


                    while ((line = reader.ReadLine()) != null)
                    {
                        index = line.IndexOf('=');

                        if (index >0)
                        {
                            name = line.Substring(0, index);
                            if (index + 1 >= line.Length)
                                value = string.Empty;
                            else
                                value = line.Substring(index + 1);

                            // and remember the history value:
                            if (data.TryGetValue(name, out items))
                            {
                                items.Add(value);
                            }
                            else
                            {
                                items = new List<string>();
                                items.Add(value);
                                data.Add(name, items);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.WriteLine("Unable to load history from file: '" + GetHistoryFileName(target) + "'.");
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }

            return data;
        }
    }
}
