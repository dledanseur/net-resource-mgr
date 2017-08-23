using System;
namespace Tools.JWT
{
    public class DecodingException: JSONValidationException
    {
		public DecodingException() { }

		public DecodingException(string message): base(message) { }

		public DecodingException(string message, Exception inner): base(message, inner) { }
	}
}
