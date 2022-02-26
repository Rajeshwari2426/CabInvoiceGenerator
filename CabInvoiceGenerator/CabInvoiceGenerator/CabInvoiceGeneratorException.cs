using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CabInvoiceGenerator
{
    public class CabInvoiceGeneratorException : Exception
    {
        public ExceptionType exceptionType;

        //Enum for Declaring constants
        public enum ExceptionType
        {
            INVALID_TIME,
            INVALID_DISTANCE
        }

        //Parametrized constructor for custom exception
        public CabInvoiceGeneratorException(ExceptionType exceptionType, string message) : base(message)
        {
            this.exceptionType = exceptionType;
        }
    }
}
