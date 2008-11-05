using EnvDTE;
using EnvDTE80;
using Pretorianie.Tytan.Core.Data;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Core.Events
{
    /// <summary>
    /// Class for monitoring Visual Studio IDE state changes.
    /// </summary>
    public class ShellEventsListener
    {
        /// <summary>
        /// Event notifying about Visual Studio IDE mode changes.
        /// </summary>
        public event ShellModeChangedHandler ModeChanged;

        private readonly DTE2 appObject;
        private readonly DTEEvents dteEvents;
        private readonly DebuggerEvents debugEvents;
        private ShellModes currentMode;

        public ShellEventsListener(DTE2 dte)
        {
            appObject = dte;
            dteEvents = dte.Events.DTEEvents;
            debugEvents = dte.Events.DebuggerEvents;
            currentMode = ShellHelper.GetMode(appObject);

            dteEvents.ModeChanged += InternalModeChanged;
            debugEvents.OnEnterBreakMode += OnEnterBreakMode;
            debugEvents.OnEnterDesignMode += OnEnterDesignMode;
            debugEvents.OnEnterRunMode += OnEnterRunMode;
        }

        void OnEnterRunMode(dbgEventReason reason)
        {
            FireEventAndUpdateMode(ShellModes.ApplicationRun);
        }

        void OnEnterDesignMode(dbgEventReason reason)
        {
            FireEventAndUpdateMode(ShellModes.Design);
        }

        void OnEnterBreakMode(dbgEventReason reason, ref dbgExecutionAction executionAction)
        {
            FireEventAndUpdateMode(ShellModes.Debug);
        }

        void InternalModeChanged(vsIDEMode lastMode)
        {
            FireEventAndUpdateMode(ShellHelper.GetMode(lastMode));
        }

        /// <summary>
        /// Gets the current state of the Visual Studio IDE.
        /// </summary>
        public ShellModes Mode
        {
            get { return currentMode; }
        }

        /// <summary>
        /// Update the current IDE mode and fire proper notification events.
        /// </summary>
        protected void FireEventAndUpdateMode(ShellModes m)
        {
            if (m != currentMode)
            {
                ShellModes prevMode = currentMode;
                currentMode = m;

                // fire notification:
                if (ModeChanged != null)
                    ModeChanged(this, appObject, currentMode, prevMode);
            }
        }
    }
}
