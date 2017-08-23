using System;

namespace Tools.JWT
{
    public class JSONValidationException : Exception
    {
		public JSONValidationException() {}

		public JSONValidationException(string message): base(message) {}

		public JSONValidationException(string message, Exception inner): base(message, inner) { }
    }

}