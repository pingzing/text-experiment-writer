using Windows.ApplicationModel;

namespace TextGameTool.Helpers
{
    public static class PackageVersionExtensions
    {
        public static void Deconstruct(this PackageVersion version, out ushort major, out ushort minor, out ushort build, out ushort revision)
        {
            major = version.Major;
            minor = version.Minor;
            build = version.Build;
            revision = version.Revision;
        }
    }
}
