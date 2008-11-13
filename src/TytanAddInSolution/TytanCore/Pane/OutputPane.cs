using System;
using EnvDTE;
using EnvDTE80;

namespace Pretorianie.Tytan.Core.Pane
{
    /// <summary>
    /// Class responsible for writing messages into Visual Studio Output Window pane with a given name.
    /// </summary>
    public class OutputPane
    {
        private readonly Window2 outputWindow;
        private readonly OutputWindowPane outputPane;
        private bool canWrite;

        /// <summary>
        /// Init constructor. Gets or creates new output pane inside Visual Studio.
        /// </summary>
        public OutputPane(DTE2 appObject, string name)
        {
            if (appObject == null)
                throw new ArgumentException("Invalid Visual Studio application object", "appObject");

            if (appObject.Windows != null)
            {
                outputWindow = appObject.Windows.Item(Constants.vsWindowKindOutput) as Window2;
                if (outputWindow != null)
                {
                    OutputWindow w = outputWindow.Object as OutputWindow;
                    if (w != null)
                    {
                        try
                        {
                            // try to open existing output pane:
                            outputPane = w.OutputWindowPanes.Item(name);
                        }
                        catch
                        {
                        }

                        // or otherwise create new one:
                        if (outputPane == null)
                            outputPane = w.OutputWindowPanes.Add(name);

                        canWrite = outputPane != null;
                    }
                }
            }
        }

        /// <summary>
        /// Writes string into current output pane.
        /// </summary>
        public void Write(string message)
        {
            if (canWrite)
                outputPane.OutputString(message);
        }

        /// <summary>
        /// Writes string into current output pane.
        /// </summary>
        public void Write(string format, params object[] args)
        {
            if (canWrite)
                outputPane.OutputString(string.Format(format, args));
        }

        /// <summary>
        /// Writes string into current output pane.
        /// </summary>
        public void WriteLine(string message)
        {
            if(canWrite)
            {
                outputPane.OutputString(message);
                outputPane.OutputString(Environment.NewLine);
            }
        }

        /// <summary>
        /// Writes string into current output pane.
        /// </summary>
        public void WriteLine(string format, params object[] args)
        {
            if (canWrite)
            {
                outputPane.OutputString(string.Format(format, args));
                outputPane.OutputString(Environment.NewLine);
            }
        }

        /// <summary>
        /// Activates given pane.
        /// </summary>
        public void Activate()
        {
            if (outputPane != null)
                outputPane.Activate();
        }

        #region Properties

        /// <summary>
        /// Checks if given <see cref="OutputPane"/> has a valid reference to Visual Studio
        /// object and can be used in writing messages.
        /// </summary>
        public bool IsValid
        {
            get { return outputPane != null; }
        }

        /// <summary>
        /// Gets or sets the indication if the messages will be shown on the output pane.
        /// </summary>
        public bool CanWrite
        {
            get { return canWrite; }
            set { canWrite = value && IsValid; }
        }

        /// <summary>
        /// Gets or sets the visibility of the <see cref="OutputWindow"/>.
        /// </summary>
        public bool Visible
        {
            get { return outputWindow != null ? outputWindow.Visible : false; }
            set { if (outputWindow != null) outputWindow.Visible = value; }
        }

        #endregion
    }
}
