using System;
using EnvDTE;
using EnvDTE80;

namespace Pretorianie.Tytan.Core.Pane
{
    /// <summary>
    /// Class providing access to Visual Studio TaskList.
    /// </summary>
    public class TaskListPane
    {
        private Window2 taskWindow;
        private EnvDTE.TaskList taskList;
        private TaskItems taskItems;
        private TaskItems2 taskItems2;

        /// <summary>
        /// Init constructor.
        /// </summary>
        public TaskListPane (DTE2 appObject)
        {
            if(appObject == null)
                throw new ArgumentException("Invalid application object", "appObject");

            if (appObject.Windows != null)
            {
                taskWindow = appObject.Windows.Item(Constants.vsWindowKindTaskList) as Window2;
                if(taskWindow != null)
                {
                    taskList = taskWindow.Object as EnvDTE.TaskList;
                    if (taskList != null)
                    {
                        taskItems = taskList.TaskItems;
                        taskItems2 = taskList.TaskItems as TaskItems2;
                    }
                }
            }
        }
    }
}
