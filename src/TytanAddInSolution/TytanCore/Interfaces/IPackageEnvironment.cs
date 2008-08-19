using System;
using Pretorianie.Tytan.Core.Data;

namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// Interface that provides environmental support for given IPackageAction.
    /// </summary>
    public interface IPackageEnvironment : IServiceProvider
    {
        /// <summary>
        /// Gets the VisualStudio IDE application object.
        /// </summary>
        EnvDTE80.DTE2 DTE
        { get; }

        /// <summary>
        /// Gets the CodeEditPoint for current active document.
        /// </summary>
        CodeEditPoint CurrentEditPoint
        { get; }

        /// <summary>
        /// Shows (or creates if needed) the specified tool window.
        /// </summary>
        bool ShowToolWindow(IPackageToolWindow toolWindow);

        /// <summary>
        /// Loads image from current package resources or satellite DLLs.
        /// </summary>
        /// <param name="name">Name of the image-resource to load.</param>
        System.Drawing.Bitmap LoadImage(string name);
    }
}
