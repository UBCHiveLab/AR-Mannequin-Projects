using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Command
{
    public byte _evCode;
    public object[] _data;

    public Command(byte evCode, object[] data)
    {
        _evCode = evCode;
        _data = data;
    }

    public override string ToString()
    {
        return "Command event: " + _evCode + " data: " + _data;
    }
}
