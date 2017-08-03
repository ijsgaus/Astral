using System;

namespace Astral.Tools.Ex
{
    
    public class PredException : Exception
    {
     
        public PredException()
        {
        }

        public PredException(string message) : base(message)
        {
        }

        public PredException(string message, Exception inner) : base(message, inner)
        {
        }

    }
}