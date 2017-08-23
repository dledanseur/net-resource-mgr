using System;
using Tools.JWT;
namespace Tools
{
    public class WrongAudienceException : JSONValidationException
    {
		public WrongAudienceException() { }

		public WrongAudienceException(string message): base(message) { }

		public WrongAudienceException(string message, Exception inner): base(message, inner) { } 
    }
}
