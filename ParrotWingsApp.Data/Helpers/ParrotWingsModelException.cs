using System;
using System.Collections.Generic;
using System.Text;

namespace ParrotWingsApp.Data.Helpers
{
    public class ParrotWingsModelException : Exception
    {
        public ParrotWingsModelException(string message) : base(message)
        {

        }

        public ParrotWingsModelException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
