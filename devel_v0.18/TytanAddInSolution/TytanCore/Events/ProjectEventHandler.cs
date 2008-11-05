using EnvDTE;

namespace Pretorianie.Tytan.Core.Events
{
    /// <summary>
    /// Handler processing notifications about project changes.
    /// </summary>
    public delegate void ProjectEventHandler(object sender, Project p);
}
