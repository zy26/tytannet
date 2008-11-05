using System;
using System.CodeDom.Compiler;
using System.Runtime.InteropServices;
using EnvDTE;
using Pretorianie.Tytan.Core.Helpers;

namespace Pretorianie.Tytan.Core.BaseGenerators
{
    /// <summary>
    /// Base code generator with site implementation.
    /// </summary>
    public abstract class BaseCodeGeneratorWithSite : BaseCodeGenerator, IObjectWithSite
    {
        private object site;
        private CodeDomProvider codeDomProvider;
        private IServiceProvider serviceProvider;

        #region IObjectWithSite Members

        /// <summary>
        /// GetSite method of IOleObjectWithSite
        /// </summary>
        /// <param name="riid">interface to get</param>
        /// <param name="ppvSite">IntPtr in which to stuff return value</param>
        public void GetSite(ref Guid riid, out IntPtr ppvSite)
        {
            if (site == null)
                throw new COMException("object is not sited", E_FAIL);

            IntPtr pUnknownPointer = Marshal.GetIUnknownForObject(site);
            IntPtr intPointer;
            Marshal.QueryInterface(pUnknownPointer, ref riid, out intPointer);

            if (intPointer == IntPtr.Zero)
                throw new COMException("site does not support requested interface", E_NOINTERFACE);

            ppvSite = intPointer;
        }

        /// <summary>
        /// SetSite method of IOleObjectWithSite
        /// </summary>
        /// <param name="pUnkSite">site for this object to use</param>
        public void SetSite(object pUnkSite)
        {
            site = pUnkSite;
            codeDomProvider = null;
            serviceProvider = null;
        }

        #endregion

        /// <summary>
        /// Demand-creates a ServiceProvider
        /// </summary>
        private IServiceProvider SiteServiceProvider
        {
            get
            {
                if (serviceProvider == null)
                    serviceProvider = site as IServiceProvider;

                return serviceProvider;
            }
        }

        /// <summary>
        /// Method to get a service by its Type
        /// </summary>
        /// <param name="serviceType">Type of service to retrieve</param>
        /// <returns>An object that implements the requested service</returns>
        protected object GetService(Type serviceType)
        {
            return GetService(SiteServiceProvider, serviceType.GUID);
        }

        /// <summary>
        /// Gets the specified service from known service provider and for given GUID.
        /// </summary>
        public static object GetService(IServiceProvider serviceProvider, Guid serviceGUID)
        {
            object objService = null;
            IntPtr objInPtr;
            Guid objSIDGuid = serviceGUID;
            Guid objIIDGuid = serviceGUID;

            int hResult = serviceProvider.QueryService(ref objSIDGuid, ref objIIDGuid, out objInPtr);
            if (hResult != 0)
                //Marshal.ThrowExceptionForHR(hResult);
                return null;

            if (objInPtr != IntPtr.Zero)
            {
                objService = Marshal.GetObjectForIUnknown(objInPtr);
                Marshal.Release(objInPtr);
            }

            return objService;
        }

        /// <summary>
        /// Returns a CodeDomProvider object for the language of the project containing
        /// the project item the generator was called on
        /// </summary>
        /// <returns>A CodeDomProvider object</returns>
        protected virtual CodeDomProvider GetCodeProvider()
        {
            if (codeDomProvider == null)
                codeDomProvider = CodeHelper.GetCodeProvider(GetProject().CodeModel.Language);
            
            return codeDomProvider;
        }

        /// <summary>
        /// Gets the default extension of the output file from the CodeDomProvider
        /// </summary>
        protected override string CodeDefaultExtension
        {
            get
            {
                CodeDomProvider codeDom = GetCodeProvider();

                string extension = codeDom.FileExtension;
                if (!string.IsNullOrEmpty(extension))
                    extension = "." + extension.TrimStart('.');

                return extension;
            }
        }

        /// <summary>
        /// Returns the EnvDTE.ProjectItem object that corresponds to the project item the code 
        /// generator was called on
        /// </summary>
        /// <returns>The EnvDTE.ProjectItem of the project item the code generator was called on</returns>
        protected ProjectItem GetProjectItem()
        {
            object p = GetService(typeof(ProjectItem));

            return (ProjectItem)p;
        }

        /// <summary>
        /// Returns the EnvDTE.Project object of the project containing the project item the code 
        /// generator was called on
        /// </summary>
        /// <returns>
        /// The EnvDTE.Project object of the project containing the project item the code generator was called on
        /// </returns>
        protected Project GetProject()
        {
            return GetProjectItem().ContainingProject;
        }

        ///// <summary>
        ///// Returns the VSLangProj.VSProjectItem object that corresponds to the project item the code 
        ///// generator was called on
        ///// </summary>
        ///// <returns>The VSLangProj.VSProjectItem of the project item the code generator was called on</returns>
        //protected VSProjectItem GetVSProjectItem()
        //{
        //    return (VSProjectItem)GetProjectItem().Object;
        //}

        ///// <summary>
        ///// Returns the VSLangProj.VSProject object of the project containing the project item the code 
        ///// generator was called on
        ///// </summary>
        ///// <returns>
        ///// The VSLangProj.VSProject object of the project containing the project item 
        ///// the code generator was called on
        ///// </returns>
        //protected VSProject GetVSProject()
        //{
        //    return (VSProject)GetProject().Object;
        //}
    }
}
