using System;
namespace Tools.Random
{
    public interface IRawRandom
    {
        byte[] GetBytes(int length); 
    }
}
