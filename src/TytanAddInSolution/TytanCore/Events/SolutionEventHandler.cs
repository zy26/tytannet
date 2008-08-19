using EnvDTE;

namespace Pretorianie.Tytan.Core.Events
{
    /// <summary>
    /// Handler processing notifications about solution changes.
    /// </summary>
    public delegate void SolutionEventHandler(object sender, Solution s);
}
