using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IIdentifier : IExpression
{
    public string Name();
    
}

public interface IIdentifiers : IExpression
{
    public IEnumerable<IIdentifier> GetIdentifiers();
}