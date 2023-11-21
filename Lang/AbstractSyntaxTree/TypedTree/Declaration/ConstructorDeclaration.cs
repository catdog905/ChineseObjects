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
    
    public TypesAwareConstructor(IScopeAwareConstructor awareConstructor) :
        this(new TypesAwareParameters(awareConstructor.Parameters()),
            new TypesAwareStatementsBlock(awareConstructor.Body())) {}
    
    public ITypesAwareStatementsBlock Body()
    {
        return _body;
    }

    public ITypesAwareParameters Parameters()
    {
        return _parameters;
    }
}