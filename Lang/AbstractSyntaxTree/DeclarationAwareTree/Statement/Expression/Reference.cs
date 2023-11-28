namespace ChineseObjects.Lang;

public interface IReference : IExpression
{
    public string Name();
    
}

public class Reference : IReference
{
    private readonly string _name;

    public Reference(string name)
    {
        _name = name;
    }
    
    public Reference(IIdentifier identifier) :
        this(identifier.Value()) {}


    public string Name()
    {
        return _name;
    }

    public IList<string> GetRepr()
    {
        return new List<string> {"REFERENCE " + _name};
    }
}