using EnvDTE;

namespace Pretorianie.Tytan.Core.Events
{
    /// <summary>
    /// Handler processing notifications about solution changes.
    /// </summary>
    public delegate void SolutionEventHandler(object sender, Solution s);

    /// <summary>
    /// Handler processing notifications about solution close queries.
    /// </summary>
    public delegate void SolutionQueryEventHandler(object sender, Solution s, ref bool bClose);
}
