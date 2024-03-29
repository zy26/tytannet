using System;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell.Interop;
using System.Diagnostics;

namespace Pretorianie.Tytan.Core.Events
{
    /// <summary>
    /// Class that listens for solution events.
    /// </summary>
    public class SolutionEventsListener : IVsSolutionEvents, IDisposable
    {
        #region Events

        /// <summary>
        /// Event fired when new project has been added to the solution.
        /// </summary>
        public event ProjectEventHandler ProjectAdded;
        /// <summary>
        /// Event fired when a project has been removed from the solution.
        /// </summary>
        public event ProjectEventHandler ProjectRemoved;
        /// <summary>
        /// Event fired when a project has been renamed.
        /// </summary>
        public event ProjectEventHandler ProjectRenamed;
        /// <summary>
        /// Event fired when a solution has been opened.
        /// </summary>
        public event SolutionEventHandler SolutionOpened;
        /// <summary>
        /// Event fired when the current solution has been closed.
        /// </summary>
        public event SolutionEventHandler SolutionClosed;
        /// <summary>
        /// Event fired when asking for solution to close.
        /// </summary>
        public event SolutionQueryEventHandler SolutionQueryClose;

        #endregion

        private IVsSolution solution;
        private uint solutionEventsCookie;

        private DTE2 appObject;
        private Events2 dteEvents;
        private SolutionEvents solutionEvents;

        /// <summary>
        /// Init constructor.
        /// Registers for solution events inside Visual Studio IDE.
        /// </summary>
        public SolutionEventsListener(IServiceProvider serviceProvider)
        {
            solution = serviceProvider.GetService(typeof (SVsSolution)) as IVsSolution;
            if (solution != null)
                solution.AdviseSolutionEvents(this, out solutionEventsCookie);
        }

        /// <summary>
        /// Init constructor.
        /// Registers for solution events inside Visual Studio IDE.
        /// </summary>
        public SolutionEventsListener(DTE2 dte)
        {
            appObject = dte;
            dteEvents = (Events2) dte.Events;
            solutionEvents = dteEvents.SolutionEvents;

            // mount even handlers, they will be detached during Dispose() method call:
            try
            {
                solutionEvents.Opened += SolutionEvents_Opened;
                solutionEvents.AfterClosing += SolutionEvents_AfterClosing;
                solutionEvents.ProjectAdded += SolutionEvents_ProjectAdded;
                solutionEvents.ProjectRemoved += SolutionEvents_ProjectRemoved;
                solutionEvents.ProjectRenamed += SolutionEvents_ProjectRenamed;
                solutionEvents.QueryCloseSolution += SolutionEvents_QueryCloseSolution;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex.Message);
                Trace.WriteLine(ex.StackTrace);
            }
        }

        #region Solution Events

        void SolutionEvents_ProjectRenamed(Project project, string oldName)
        {
            if (ProjectRenamed != null)
                ProjectRenamed(this, project);
        }

        void SolutionEvents_ProjectRemoved(Project project)
        {
            if (ProjectRemoved != null)
                ProjectRemoved(this, project);
        }

        void SolutionEvents_ProjectAdded(Project project)
        {
            if (ProjectAdded != null)
                ProjectAdded(this, project);
        }

        void SolutionEvents_AfterClosing()
        {
            if (SolutionClosed != null)
                SolutionClosed(this, appObject.Solution);
        }

        void SolutionEvents_Opened()
        {
            if (SolutionOpened != null)
                SolutionOpened(this, appObject.Solution);
        }

        void SolutionEvents_QueryCloseSolution(ref bool fCancel)
        {
            if (SolutionQueryClose != null)
                SolutionQueryClose(this, appObject.Solution, ref fCancel);
        }

        #endregion

        #region IVsSolutionEvents Members

        int IVsSolutionEvents.OnAfterCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnAfterLoadProject(IVsHierarchy pStubHierarchy, IVsHierarchy pRealHierarchy)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnAfterOpenProject(IVsHierarchy pHierarchy, int fAdded)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnAfterOpenSolution(object pUnkReserved, int fNewSolution)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnBeforeCloseProject(IVsHierarchy pHierarchy, int fRemoved)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnBeforeCloseSolution(object pUnkReserved)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnBeforeUnloadProject(IVsHierarchy pRealHierarchy, IVsHierarchy pStubHierarchy)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnQueryCloseProject(IVsHierarchy pHierarchy, int fRemoving, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnQueryCloseSolution(object pUnkReserved, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        int IVsSolutionEvents.OnQueryUnloadProject(IVsHierarchy pRealHierarchy, ref int pfCancel)
        {
            return VSConstants.S_OK;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Release used VS IDE resources.
        /// </summary>
        public void Dispose()
        {
            if (solution != null && solutionEventsCookie != 0)
            {
                GC.SuppressFinalize(this);
                solution.UnadviseSolutionEvents(solutionEventsCookie);
                solutionEventsCookie = 0;
                solution = null;
            }

            if (solutionEvents != null)
            {
                solutionEvents.Opened -= SolutionEvents_Opened;
                solutionEvents.AfterClosing -= SolutionEvents_AfterClosing;
                solutionEvents.ProjectAdded -= SolutionEvents_ProjectAdded;
                solutionEvents.ProjectRemoved -= SolutionEvents_ProjectRemoved;
                solutionEvents.ProjectRenamed -= SolutionEvents_ProjectRenamed;
                solutionEvents.QueryCloseSolution -= SolutionEvents_QueryCloseSolution;
                solutionEvents = null;
                dteEvents = null;
                appObject = null;
            }
        }

        #endregion
    }
}
