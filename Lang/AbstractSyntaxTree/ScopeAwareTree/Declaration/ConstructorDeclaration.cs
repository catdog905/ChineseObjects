namespace ChineseObjects.Lang;

public interface IScopeAwareConstructorDeclaration : IConstructorDeclaration, IScopeAwareAstNode
{
    public new IScopeAwareParameters Parameters();
    public new IScopeAwareStatementsBlock Body();
}

public class ScopeAwareConstructorDeclaration : IScopeAwareConstructorDeclaration
{
    private readonly Scope _scope;
    private readonly ScopeAwareParameters _parameters;
    private readonly ScopeAwareStatementsBlock _body;

    private ScopeAwareConstructorDeclaration(Scope scope, ScopeAwareParameters parameters, ScopeAwareStatementsBlock body)
    {
        _scope = scope;
        _parameters = parameters;
        _body = body;
    }

    private ScopeAwareConstructorDeclaration(ScopeWithFields scope, IConstructorDeclaration constructorDeclaration) :
        this(
            scope, 
            new ScopeAwareParameters(scope, constructorDeclaration.Parameters()), 
            new ScopeAwareStatementsBlock(scope, constructorDeclaration.Body())) {}
    
    public ScopeAwareConstructorDeclaration(Scope scope, IConstructorDeclaration constructorDeclaration) :
        this(
            new ScopeWithFields(scope, constructorDeclaration.Parameters().GetParameters()),
            constructorDeclaration) {}
    
    class ScopeWithFields : Scope
    {
        public ScopeWithFields(Scope scope, IEnumerable<IParameter> parameters) :
            base(scope, 
                parameters.ToDictionary(
                    parameter => parameter.Name(),
                    parameter => new Reference(parameter.Name(), new Type(scope, parameter.TypeName())))
                ) {}
    }

    public Scope Scope()
    {
        return _scope;
    }


    IParameters IConstructorDeclaration.Parameters()
    {
        return Parameters();
    }

    public IScopeAwareStatementsBlock Body()
    {
        return _body;
    }

    public IScopeAwareParameters Parameters()
    {
        return _parameters;
    }

    IStatementsBlock IConstructorDeclaration.Body()
    {
        return Body();
    }
}