namespace ChineseObjects.Lang;

public interface ITypedClassInstantiation : ITypedExpression
{
    public string ClassName();
    public ITypesAwareArguments Arguments();
}

public class TypedClassInstantiation : ITypedClassInstantiation
{
    private readonly Type _type;
    private readonly string _className;
    private readonly ITypesAwareArguments _arguments;

    public TypedClassInstantiation(
        Type type, 
        string className, 
        ITypesAwareArguments arguments)
    {
        _type = type;
        _className = className;
        _arguments = arguments;
    }

    public TypedClassInstantiation(IScopeAwareClassInstantiation classInstantiation) :
        this(
            new Type(classInstantiation.Scope(), 
                classInstantiation.ClassName()), 
            classInstantiation.ClassName().Value(),
            new TypesAwareArguments(classInstantiation.Arguments())) {}


    public Type Type()
    {
        return _type;
    }

    public string ClassName()
    {
        return _className;
    }

    public ITypesAwareArguments Arguments()
    {
        return _arguments;
    }
}