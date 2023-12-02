using ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Declaration;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration.Parameter;
using ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Statement;

namespace ChineseObjects.Lang.AbstractSyntaxTree.TypedTree.Declaration;

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
    
    public ITypesAwareStatementsBlock Body()
    {
        return _body;
    }

    public ITypesAwareParameters Parameters()
    {
        return _parameters;
    }

    public IList<string> GetRepr()
    {
        return new DeclarationAwareTree.Declaration.ConstructorDeclaration(this).GetRepr();
    }
}