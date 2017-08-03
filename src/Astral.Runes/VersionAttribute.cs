using System;

namespace Astral.Runes
{
    [AttributeUsage(AttributeTargets.Interface)]
    public class VersionAttribute : Attribute
    {
        public VersionAttribute(string version)
        {
            Version = Version.Parse(version);
        }

        public Version Version { get; }
    }
}