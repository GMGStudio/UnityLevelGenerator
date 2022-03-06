using System;

public class NoFreePosition : Exception
{
    public NoFreePosition(string message) : base (message)
    {
    }
}
