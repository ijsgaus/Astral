using System;

namespace Astral.Tools.Ex
{
    [AttributeUsage(AttributeTargets.Struct)]
    public class PredErrorAttribute : Attribute
    {
        public PredErrorAttribute(string format)
        {
            Format = format;
        }

        public string Format { get; }
    }
}