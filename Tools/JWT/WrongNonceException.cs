using System;
namespace Tools.JWT
{
    public class WrongNonceException : JSONValidationException
    {
		public WrongNonceException() { }

		public WrongNonceException(string message): base(message) { }

		public WrongNonceException(string message, Exception inner): base(message, inner) { }
	}
}
