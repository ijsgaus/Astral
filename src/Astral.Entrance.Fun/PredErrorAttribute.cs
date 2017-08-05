using System;

namespace Astral.Fun
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