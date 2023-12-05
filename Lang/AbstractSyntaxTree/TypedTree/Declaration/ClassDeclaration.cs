using System.Collections.Immutable;

namespace ChineseObjects.Lang.Declaration;

public interface ITypesAwareClassDeclaration : ITypesAwareAstNode
{
    public string ClassName();
    public Type SelfType();
    public IEnumerable<Type> ParentClassNames();
    public IEnumerable<ITypesAwareConstructor> ConstructorDeclarations();
    public IEnumerable<ITypedVariable> VariableDeclarations();
    public IEnumerable<ITypesAwareMethod> MethodDeclarations();
}

public class TypesAwareClassDeclaration : ITypesAwareClassDeclaration
{
    private readonly Type _selfType;
    private readonly IEnumerable<Type> _parentClassNames;
    private readonly IEnumerable<ITypesAwareConstructor> _constructorDeclarations;
    private readonly IEnumerable<ITypedVariable> _variableDeclarations;
    private readonly IEnumerable<ITypesAwareMethod> _methodDeclarations;

    public TypesAwareClassDeclaration(
        Type selfType,
        IEnumerable<Type> parentClassNames, 
        IEnumerable<ITypesAwareConstructor> constructorDeclarations, 
        IEnumerable<ITypedVariable> variableDeclarations, 
        IEnumerable<ITypesAwareMethod> methodDeclarations)
    {
        _selfType = selfType;
        _parentClassNames = parentClassNames;
        _variableDeclarations = variableDeclarations;
        var typesAwareMethods = methodDeclarations.ToList();
        _methodDeclarations = typesAwareMethods;

        ImmutableList<ITypesAwareConstructor> typesAwareConstructors = constructorDeclarations.ToImmutableList();
        if (typesAwareConstructors.Count() == 0)
        {
            typesAwareConstructors = typesAwareConstructors.Add(
                new TypesAwareConstructor());
        }
        _constructorDeclarations = typesAwareConstructors;
        if (typesAwareConstructors
                .GroupBy(decl => decl.Parameters())
                .Count(group => group.Count() > 1) != 0)
            throw new DuplicatedConstructorException(_selfType.TypeName().Value());
        if (typesAwareMethods
                .GroupBy(decl => (decl.MethodName(), decl.Parameters()))
                .Count(group => group.Count() > 1) != 0)
            throw new DuplicatedMethodException(_selfType.TypeName().Value());
    }

    public TypesAwareClassDeclaration(IScopeAwareClass scopeAwareClass) :
        this(
            new Type(scopeAwareClass.Scope(), scopeAwareClass.ClassName().Value()),
            scopeAwareClass.ParentClassNames()
                .Select(parentClassName => new Type(scopeAwareClass.Scope(), parentClassName))
                .ToList(),
            scopeAwareClass.ConstructorDeclarations()
                .Select(decl => new TypesAwareConstructor(decl))
                .ToList(),
            scopeAwareClass.VariableDeclarations()
                .Select(decl => new TypedVariable(decl))
                .ToList(),
            scopeAwareClass.MethodDeclarations()
                .Select(decl => new TypesAwareMethod(decl))
                .ToList()) {}


    public string ClassName()
    {
        return _selfType.TypeName().Value();
    }

    public Type SelfType()
    {
        return _selfType;
    }

    public IEnumerable<Type> ParentClassNames()
    {
        return _parentClassNames;
    }

    public IEnumerable<ITypesAwareConstructor> ConstructorDeclarations()
    {
        return _constructorDeclarations;
    }

    public IEnumerable<ITypedVariable> VariableDeclarations()
    {
        return _variableDeclarations;
    }

    public IEnumerable<ITypesAwareMethod> MethodDeclarations()
    {
        return _methodDeclarations;
    }
    
    
}

public interface ITypedThis : ITypedExpression {}

public class TypedThis : ITypedThis
{
    private readonly Type _type;
    
    public TypedThis(Type type)
    {
        _type = type;
    }
    
    public TypedThis(IScopeAwareThis sThis) : this(sThis.Scope().GetValue("this").Type()) {}

    public Type Type() => _type;
    
    public T AcceptVisitor<T>(CodeGen.ITypesAwareStatementVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}


public class DuplicatedConstructorException : Exception
{
    public DuplicatedConstructorException(string className) : 
        base(className + " have two constructors with the same signature") { }
}

public class DuplicatedMethodException : Exception
{
    public DuplicatedMethodException(string className) : 
        base(className + " have two method with the same signature") { }
}
