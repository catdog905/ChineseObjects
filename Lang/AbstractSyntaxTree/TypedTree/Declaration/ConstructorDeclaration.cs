namespace ChineseObjects.Lang.Declaration;

public interface ITypesAwareConstructor : ITypesAwareAstNode
{
    public ITypesAwareParameters Parameters();
    public ITypesAwareStatementsBlock Body();
}

public class TypesAwareConstructor : ITypesAwareConstructor
{
    private readonly ITypesAwareParameters _parameters;
    private readonly ITypesAwareStatementsBlock _body;

    public TypesAwareConstructor(ITypesAwareParameters parameters, ITypesAwareStatementsBlock body)
    {
        _parameters = parameters;
        _body = body;
    }
    
    public TypesAwareConstructor(IScopeAwareConstructor scopeAwareConstructor) :
        this(new TypesAwareParameters(scopeAwareConstructor.Parameters()),
            new TypesAwareStatementsBlock(scopeAwareConstructor.Body())) {}

    public TypesAwareConstructor() :
        this(new TypesAwareParameters(), new TypesAwareStatementsBlock()) {}

    public ITypesAwareStatementsBlock Body()
    {
        return _body;
    }

    public ITypesAwareParameters Parameters()
    {
        return _parameters;
    }
}