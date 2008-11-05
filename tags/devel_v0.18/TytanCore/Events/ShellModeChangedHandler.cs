using EnvDTE80;
using Pretorianie.Tytan.Core.Data;

namespace Pretorianie.Tytan.Core.Events
{
    /// <summary>
    /// Delegate to function responsible for handling notification about Visual Studio IDE state changes.
    /// </summary>
    /// <param name="sender">Sender of the event.</param>
    /// <param name="appObject">Current Visual Studio IDE object.</param>
    /// <param name="currentMode">Current mode of the IDE.</param>
    /// <param name="oldMode">Previous mode of the IDE.</param>
    public delegate void ShellModeChangedHandler(object sender,
        DTE2 appObject, ShellModes currentMode, ShellModes oldMode);
}
