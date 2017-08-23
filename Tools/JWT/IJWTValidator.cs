using System;
using System.Collections.Generic;
namespace Tools.JWT
{
    public interface IJWTValidator
    {
        Dictionary<string,object> Validate(string jwt, string audience, string nonce);
	}
}
