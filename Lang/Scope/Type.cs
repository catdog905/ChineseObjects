namespace ChineseObjects.Lang;

public class Type
{
    private IClassDeclaration _classDeclaration;

    public Type(IClassDeclaration classDeclaration)
    {
        _classDeclaration = classDeclaration;
    }

    public Type(Scope scope, string className) : this(scope.GetType(className)._classDeclaration) {}
    
    public Type(Scope scope, IIdentifier className) : this(scope, className.Value()) {}

    public Type(Scope scope, IScopeAwareIdentifier className) : 
        this(scope, className.Value()) {}

    public IIdentifier TypeName()
    {
        return _classDeclaration.ClassName();
    }

    public Type MethodCallReturnType(Scope scope, string methodName)
    {
        var declarations = _classDeclaration.MethodDeclarations()
            .Where(methodDeclaration => methodDeclaration.MethodName().Value() == methodName)
            .ToList();
        try
        {
            return new Type(scope, declarations.First().ReturnTypeName());
        }
        catch (InvalidOperationException e)
        {
            throw new NoSuchMethodException(methodName, _classDeclaration.ClassName().Value(), e);
        }
    }

    protected bool Equals(Type other)
    {
        return _classDeclaration.Equals(other._classDeclaration);
    }

    public override bool Equals(object? obj)
    {
        if (ReferenceEquals(null, obj)) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != this.GetType()) return false;
        return Equals((Type)obj);
    }

    public override int GetHashCode()
    {
        return _classDeclaration.GetHashCode();
    }

    public static bool operator ==(Type? left, Type? right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Type? left, Type? right)
    {
        return !Equals(left, right);
    }

    public override string ToString()
    {
        return $"{_classDeclaration.GetHashCode()}";
    }
}

public class NoSuchMethodException : Exception
{
    public NoSuchMethodException(
        string methodName, 
        string className, 
        InvalidOperationException invalidOperationException) :
        base("Method with name '" + methodName + "' wasn't found in " + className, invalidOperationException)
    { }
}