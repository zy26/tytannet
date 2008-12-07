using EnvDTE;
using Pretorianie.Tytan.Actions.Misc;

namespace Pretorianie.Tytan.Actions.Internals
{
    internal class ReferenceProjectRemovedTask : BaseReferenceTask
    {
        private readonly Project p;

        public ReferenceProjectRemovedTask(ReferenceProjectAction action, Project p)
            : base(action)
        {
            this.p = p;
        }

        protected override void Execute()
        {
            action.OnProjectRemoved(p);
        }
    }
}
