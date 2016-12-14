using System;
using System.Linq;
using System.Reflection;
using Microsoft.ApplicationInsights;

namespace Dematt.Airy.ApplicationInsights.Owin
{
    internal class SdkVersionUtils
    {
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
