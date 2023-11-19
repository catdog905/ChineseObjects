namespace ChineseObjects.Lang;

public interface IScopeAwareConstructor : IConstructor, IScopeAwareAstNode
{
    public IScopeAwareParameters Parameters();
    public IScopeAwareStatementsBlock Body();
}

public class ScopeAwareConstructor : IScopeAwareConstructor
{
    private readonly Scope _scope;
    private readonly ScopeAwareParameters _parameters;
    private readonly ScopeAwareStatementsBlock _body;

    private ScopeAwareConstructor(Scope scope, ScopeAwareParameters parameters, ScopeAwareStatementsBlock body)
    {
        _scope = scope;
        _parameters = parameters;
        _body = body;
    }

    private ScopeAwareConstructor(ScopeWithFields scope, IConstructorDeclaration constructorDeclaration) :
        this(
            scope, 
            new ScopeAwareParameters(scope, constructorDeclaration.Parameters()), 
            new ScopeAwareStatementsBlock(scope, constructorDeclaration.Body())) {}
    
    public ScopeAwareConstructor(Scope scope, IConstructorDeclaration constructorDeclaration) :
        this(
            new ScopeWithFields(scope, constructorDeclaration.Parameters().GetParameters()),
            constructorDeclaration) {}
    
    class ScopeWithFields : Scope
    {
        public ScopeWithFields(Scope scope, IEnumerable<IParameterDeclaration> parameters) :
            base(scope, 
                parameters.ToDictionary(
                    parameter => parameter.Name().Name(),
                    parameter => new Reference(parameter.Name(), new Type(scope, parameter.TypeName())))
                ) {}
    }

    public Scope Scope()
    {
        return _scope;
    }

    public IScopeAwareStatementsBlock Body()
    {
        return _body;
    }

    public IScopeAwareParameters Parameters()
    {
        return _parameters;
    }
}