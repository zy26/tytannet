using Pretorianie.Tytan.Actions.Misc;

namespace Pretorianie.Tytan.Actions.Internals
{
    internal class ReferenceSolutionOpenedTask : BaseReferenceTask
    {
        public ReferenceSolutionOpenedTask(ReferenceProjectAction action)
            : base(action)
        {
        }

        protected override void Execute()
        {
            action.OnSolutionOpened();
        }
    }
}
