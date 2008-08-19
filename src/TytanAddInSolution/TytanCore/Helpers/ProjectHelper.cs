using System;
using System.Collections.Generic;
using EnvDTE;
using EnvDTE80;
using Pretorianie.Tytan.Core.Interfaces;

namespace Pretorianie.Tytan.Core.Helpers
{
    /// <summary>
    /// Class that exposes useful function around Project object.
    /// </summary>
    public static class ProjectHelper
    {
        /// <summary>
        /// Returns currently selected project inside the SolutionExplorer window.
        /// </summary>
        public static Project GetSelectedProject(DTE2 appObject)
        {
            // or from SolutionExplorer:
            Array selectedProjects = (Array)appObject.ActiveSolutionProjects;

            if (selectedProjects != null && selectedProjects.Length > 0)
            {
                return selectedProjects.GetValue(0) as Project;
            }

            return null;
        }

        /// <summary>
        /// Returns full path to the output binary of the given project in current configuration.
        /// </summary>
        public static string GetOutputPath(Project p)
        {
            if (p == null || p.Properties == null
                || p.ConfigurationManager == null || p.ConfigurationManager.ActiveConfiguration == null ||
                p.ConfigurationManager.ActiveConfiguration.Properties == null)
                return null;

            string localPath;

            try
            {
                localPath = (string) p.Properties.Item("LocalPath").Value;
            }
            catch
            {
                localPath = string.Empty;
            }

            return localPath +
                   (string)p.ConfigurationManager.ActiveConfiguration.Properties.Item("OutputPath").Value +
                   (string)p.Properties.Item("OutputFileName").Value;
        }

        /// <summary>
        /// Returns the list of current projects inside active solution.
        /// </summary>
        public static IList<Project> GetAllProjects(DTE2 appObject)
        {
            if (appObject == null || appObject.DTE.Solution.Projects.Count == 0)
                return null;

            IList<Project> projects = new List<Project>();

            foreach (Project p in appObject.DTE.Solution.Projects)
                projects.Add(p);

            return projects;
        }

        /// <summary>
        /// Returns the list of managed projects inside active solution.
        /// </summary>
        public static IList<Project> GetManagedProjects(DTE2 appObject)
        {
            if (appObject == null || appObject.DTE.Solution.Projects.Count == 0)
                return null;

            IList<Project> projects = new List<Project>();

            foreach (Project p in appObject.DTE.Solution.Projects)
                if (IsManaged(p))
                    projects.Add(p);

            return projects;
        }

        /// <summary>
        /// Returns 'true' if given project is managed.
        /// </summary>
        public static bool IsManaged(Project p)
        {
            try
            {
                CodeModelLanguages language = CodeHelper.GetCodeLanguage(p.CodeModel.Language);

                return language == CodeModelLanguages.VisualBasic ||
                       language == CodeModelLanguages.VisualCSharp;
            }
            catch
            {
                return false;
            }
        }
    }
}
