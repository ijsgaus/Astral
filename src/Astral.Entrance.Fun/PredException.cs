using System;

namespace Astral.Fun
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