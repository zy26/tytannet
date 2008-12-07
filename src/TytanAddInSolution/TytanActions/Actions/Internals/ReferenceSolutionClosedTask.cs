using Pretorianie.Tytan.Actions.Misc;

namespace Pretorianie.Tytan.Actions.Internals
{
    internal class ReferenceSolutionClosedTask : BaseReferenceTask
    {
        public ReferenceSolutionClosedTask(ReferenceProjectAction action)
            : base(action)
        {
        }

        protected override void Execute()
        {
            action.OnSolutionClosed();
        }
    }
}
