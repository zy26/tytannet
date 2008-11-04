using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Pretorianie.Tytan.Core.Data
{
    /// <devdoc>
    ///     This class derives from container to provide a service provider
    ///     connection to the package.
    /// </devdoc>
    sealed class PackageContainer : Container
    {
        private IUIService uis;
        private AmbientProperties ambientProperties;

        private readonly IServiceProvider provider;

        /// <devdoc>
        ///     Creates a new container using the given service provider.
        /// </devdoc>
        internal PackageContainer(IServiceProvider provider)
        {
            this.provider = provider;
        }

        /// <devdoc>
        ///     Override to GetService so we can route requests
        ///     to the package's service provider.
        /// </devdoc>
        protected override object GetService(Type serviceType)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            if (provider != null)
            {
                if (serviceType == typeof(AmbientProperties))
                {
                    if (uis == null)
                        uis = (IUIService) provider.GetService(typeof (IUIService));

                    if (ambientProperties == null)
                        ambientProperties = new AmbientProperties();

                    if (uis != null)
                        // update the _ambientProperties in case the styles have changed
                        // since last time.
                        ambientProperties.Font = (Font)uis.Styles["DialogFont"];

                    return ambientProperties;
                }
                object service = provider.GetService(serviceType);

                if (service != null)
                    return service;
            }

            return base.GetService(serviceType);
        }
    }
}
