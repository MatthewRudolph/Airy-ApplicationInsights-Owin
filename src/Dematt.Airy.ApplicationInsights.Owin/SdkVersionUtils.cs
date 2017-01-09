using System;
using System.Linq;
using System.Reflection;
using Microsoft.ApplicationInsights;

namespace Dematt.Airy.ApplicationInsights.Owin
{
    /// <summary>
    /// Utilities to get version information for internal use.
    /// </summary>
    internal class SdkVersionUtils
    {
        /// <summary>
        /// Gets the version of the Application Insights library being used.
        /// This is a copy of the code from the Microsoft.ApplicationInsights.Web repository on GitHub, with minor changes as show in the in line comments.
        /// </summary>
        internal static string GetSdkVersion(string versionPrefix)
        {
            // Changed to use the type of TelemetryClient to get the SDK version as we want to report the ApplicationInsights SDK version not the version of our library.
            string versionStr = typeof(TelemetryClient).Assembly.GetCustomAttributes(false)
                    .OfType<AssemblyFileVersionAttribute>()
                    .First()
                    .Version;

            Version version = new Version(versionStr);
            return (versionPrefix ?? string.Empty) + version.ToString(3) + "-" + version.Revision;
        }
    }
}
