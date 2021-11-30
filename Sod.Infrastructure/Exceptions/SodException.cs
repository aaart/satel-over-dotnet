using System;

namespace Sod.Infrastructure.Exceptions
{
    public class SodException : Exception
    {
        public SodException()
        {
        }
        
        public SodException(string message) 
            : base(message)
        {
        }

        public SodException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }
    }
}