using System;
using EnvDTE80;
using Pretorianie.Tytan.Core.Data;

namespace Pretorianie.Tytan.Core.Interfaces
{
    /// <summary>
    /// Interface that provides environmental support for given <see cref="IPackageAction"/>.
    /// </summary>
    public interface IPackageEnvironment : IServiceProvider
    {
        /// <summary>
        /// Gets the Visual Studio IDE application object.
        /// </summary>
        DTE2 DTE
        { get; }

        /// <summary>
        /// Gets the current version of the Visual Studio IDE.
        /// </summary>
        ShellVersions Version
        { get; }

        /// <summary>
        /// Gets the current state of the Visual Studio IDE.
        /// </summary>
        ShellModes Mode
        { get; }

        /// <summary>
        /// Gets the <see cref="CodeEditPoint"/> for current active document.
        /// </summary>
        CodeEditPoint CurrentEditPoint
        { get; }

        /// <summary>
        /// Gets the current info about given Visual Studio AddIn.
        /// </summary>
        PackageInfo Info
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

        /// <summary>
        /// Sets given object as a source provide of 'Properties Window' inside Visual Studio IDE.
        /// </summary>
        /// <param name="currentWindow">Source window, that requests set.</param>
        /// <param name="sourceObject">Object, which properties will be edited.</param>
        void SelectAtProperties(Window2 currentWindow, object sourceObject);
    }
}
