using System;
using Pretorianie.Database.Imports;
using System.Diagnostics;

namespace Pretorianie.Tytan.Database.Helpers
{
    /// <summary>
    /// Class that helps in graphical edition of the database connection.
    /// It displays the standard system dialog and produces the connection string acceptable all over across .NET platform.
    /// This might be used to connect to any version of SQL Server, Oracle and even MS Access / Excel.
    /// </summary>
    public static class ConnectionHelper
    {
        /// <summary>
        /// Asks user with nice standard dialog-box about the settings of the connection to the database.
        /// It returns 'true' when user defined a (valid) connection and pressed [OK] button,
        /// otherwise 'false'. In case of edition of an existing connection, pass it via
        /// <paramref name="initialConnectionString"/> parameter.
        /// </summary>
        public static bool PromptConnectionString(string initialConnectionString, out string userConnectionString)
        {
            DataLinks sysConnectionDialog = new DataLinks();
            _Connection adoConnection;

            if (string.IsNullOrEmpty(initialConnectionString))
            {
                // get the connection object from the dialog:
                adoConnection = (_Connection)sysConnectionDialog.PromptNew();
            }
            else
            {
                try
                {
                    // create the connection object and pass it to the dialog:
                    adoConnection = new ConnectionClass();
                    adoConnection.ConnectionString = initialConnectionString;
                    object oConnection = adoConnection;

                    // and if user clicked [NO] on the dialog, release the connection object
                    // to return 'false' and empty string...
                    if (!sysConnectionDialog.PromptEdit(ref oConnection))
                        adoConnection = null;
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.Message);
                    Trace.WriteLine(ex.StackTrace);

                    // create the connection object without passing the connection string
                    // as this probably was incorrect and caused an error:
                    adoConnection = (_Connection)sysConnectionDialog.PromptNew();
                }
            }

            if (adoConnection != null)
            {
                userConnectionString = adoConnection.ConnectionString;
                return true;
            }

            // otherwise return the default connection info:
            userConnectionString = null;
            return false;
        }
    }
}
