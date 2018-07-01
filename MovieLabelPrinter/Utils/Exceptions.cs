using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Exception handling basics

namespace Utils
{
    // Set of basic exception classes for the application
    // Currently derived classes don't contain any new functionality

    public class MovieLabelingException : Exception
    {
        public MovieLabelingException(string message)
            : base(message)
        {
        }
    }

    public class MovieLabelingInternalException : MovieLabelingException
    {
        public MovieLabelingInternalException(string message)
            : base(message)
        {
        }
    }

    public class MovieLabelingInvalidParameterException : MovieLabelingException
    {
        public MovieLabelingInvalidParameterException(string message)
            : base(message)
        {
        }
    }
}
