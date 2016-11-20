using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Hemlock.Models.Exceptions
{
    public class InvalidUserException : Exception
    {
        public InvalidUserException(string message) : base(message)
        {
        }
    }
}