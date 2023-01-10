namespace EasyCI.Domain.Logic.Helpers
{
    public static class CurrentOsHelper
    {
        public static bool IsWindows()
        {
            var os = Environment.OSVersion;
            return os.Platform == PlatformID.Win32NT;
        }
    }
}
