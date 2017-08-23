using System;
namespace Tools.JWT
{
    public class TokenExpiredException : JSONValidationException
    {
		public TokenExpiredException() { }

		public TokenExpiredException(string message): base(message) { }

		public TokenExpiredException(string message, Exception inner): base(message, inner) { }
	}
}
