using EasyCI.Domain.Contracts.Models;
using EasyCI.Domain.Contracts.Services;
using EasyCI.Domain.Logic.Helpers;
using System.Diagnostics;

namespace EasyCI.Domain.Logic.Services
{
    public class AppPathResolver : IAppPathResolver
    {
        public string ResolveAppPath(AvailableApps app)
        {
            switch (app)
            {
                case AvailableApps.Docker:
                    return IsWindows() ? GetAppPathForWindows(AvailableApps.Docker) : GetAppPathForLinux(AvailableApps.Docker);
                case AvailableApps.Git:
                    return IsWindows() ? GetAppPathForWindows(AvailableApps.Git) : GetAppPathForLinux(AvailableApps.Git);
                default:
                    throw new ArgumentException("Unknown app");
            }
        }

        private bool IsWindows()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }

        private string GetAppPathForWindows(AvailableApps app)
        {
            var appName = app.ToString().ToLower();
            string path = Environment.GetEnvironmentVariable("PATH").AssertValue().ToLower();
            string? appPath = path.Split(';').FirstOrDefault(p => p.Contains(appName));

            return appPath.AssertValue() + $"\\{appName}.exe";
        }

        private string GetAppPathForLinux(AvailableApps app)
        {
            var appName = app.ToString().ToLower();
            using (var process = new Process())
            {
                process.StartInfo.FileName = "which";
                process.StartInfo.Arguments = appName;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.UseShellExecute = false;
                process.Start();

                var appPath = process.StandardOutput.ReadToEnd().Trim();
                process.WaitForExit();

                return appPath.AssertValue();
            }
        }
    }
}
