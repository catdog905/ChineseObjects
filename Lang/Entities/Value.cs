using System.Collections.Immutable;

namespace ChineseObjects.Lang.Entities;

public class Value
{
    public readonly string TypeName;

    public Value(string type)
    {
        TypeName = type;
    }
}