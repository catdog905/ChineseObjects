namespace ChineseObjects.Lang;

public interface IConstructorDeclaration : MemberDeclaration
{
    public Parameters Parameters();
    public StatementsBlock Body();
}

public interface IScopeAwareConstructorDeclaration : IConstructorDeclaration, IScopeAwareAstNode
{
    new ScopeAwareParameters Parameters();
    new ScopeAwareStatementsBlock Body();
}

public class ConstructorDeclaration : IConstructorDeclaration, IHumanReadable
{
    private readonly Parameters _parameters;
    private readonly StatementsBlock _body;

    public ConstructorDeclaration(Parameters parameters, StatementsBlock statementsBlock)
    {
        _parameters = parameters;
        _body = statementsBlock;
    }

    public override string ToString()
    {
        return "This(" + _parameters + ") {" + _body + "}";
    }

    public IList<string> GetRepr()
    {
        var ans = new List<string> {"CONSTRUCTOR"};
        foreach(Parameter param in _parameters.GetParameters())
        {
            ans.AddRange(param.GetRepr().Select(s => "| " + s));
        }
        ans.Add("|--");
        foreach(IStatement stmt in _body._statements) {
            ans.AddRange(stmt.GetRepr().Select(s => "| " + s));
        }
        return ans;
    }

    public Parameters Parameters()
    {
        return _parameters;
    }

    public StatementsBlock Body()
    {
        return _body;
    }
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

    private ScopeAwareConstructorDeclaration(ScopeWithFields scope, ConstructorDeclaration constructorDeclaration) :
        this(
            scope, 
            new ScopeAwareParameters(scope, constructorDeclaration.Parameters()), 
            new ScopeAwareStatementsBlock(scope, constructorDeclaration.Body())) {}
    
    public ScopeAwareConstructorDeclaration(Scope scope, ConstructorDeclaration constructorDeclaration) :
        this(
            new ScopeWithFields(scope, constructorDeclaration.Parameters().GetParameters()),
            constructorDeclaration) {}
    
    class ScopeWithFields : Scope
    {
        public ScopeWithFields(Scope scope, IEnumerable<Parameter> parameters) :
            base(scope, 
                parameters.ToDictionary(
                    parameter => parameter.Name(),
                    parameter => new Reference(parameter.Name(), new Type(scope, parameter.Type())))
                ) {}
    }

    public Scope Scope()
    {
        return _scope;
    }

    ScopeAwareParameters IScopeAwareConstructorDeclaration.Parameters()
    {
        return _parameters;
    }

    ScopeAwareStatementsBlock IScopeAwareConstructorDeclaration.Body()
    {
        return _body;
    }

    Parameters IConstructorDeclaration.Parameters()
    {
        throw new NotImplementedException();
    }

    StatementsBlock IConstructorDeclaration.Body()
    {
        throw new NotImplementedException();
    }
}