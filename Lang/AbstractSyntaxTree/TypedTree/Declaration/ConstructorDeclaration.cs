namespace ChineseObjects.Lang.Declaration;

public interface ITypesAwareConstructorDeclaration : IConstructorDeclaration, ITypesAwareAstNode
{
    public new ITypesAwareParameters Parameters();
    public new ITypesAwareStatementsBlock Body();
}

public class TypesAwareConstructorDeclaration : ITypesAwareConstructorDeclaration
{
    private readonly ITypesAwareParameters _parameters;
    private readonly ITypesAwareStatementsBlock _body;

    public TypesAwareConstructorDeclaration(ITypesAwareParameters parameters, ITypesAwareStatementsBlock body)
    {
        _parameters = parameters;
        _body = body;
    }
    
    public TypesAwareConstructorDeclaration(IScopeAwareConstructorDeclaration constructorDeclaration) :
        this(new TypesAwareParameters(constructorDeclaration.Parameters()),
            new TypesAwareStatementsBlock(constructorDeclaration.Body())) {}


    IParameters IConstructorDeclaration.Parameters()
    {
        return Parameters();
    }

    public ITypesAwareStatementsBlock Body()
    {
        return _body;
    }

    public ITypesAwareParameters Parameters()
    {
        return _parameters;
    }

    IStatementsBlock IConstructorDeclaration.Body()
    {
        return Body();
    }
}