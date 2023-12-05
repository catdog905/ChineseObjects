using ChineseObjects.Lang.Declaration;

namespace ChineseObjects.Lang;

public class Type
{
    private readonly IClassDeclaration _classDeclaration;

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

    public Type MethodCallReturnType(IScopeAwareMethodCall methodCall)
    {
        ITypesAwareArguments methodCallArguments = new TypesAwareArguments(methodCall.Arguments());
        bool MethodSignatureCheck(IMethodDeclaration methodDeclaration)
        {
            if (!methodDeclaration.MethodName().Value().Equals(methodCall.MethodName().Value()))
                return false;
            if (methodDeclaration.Parameters().GetParameters().Count() != methodCallArguments.Values().Count())
                return false;
            foreach (var (parameter, argument) in methodDeclaration.Parameters().GetParameters()
                         .Zip(methodCallArguments.Values())) 
            {
                if (!parameter.TypeName().Equals(argument.Type().TypeName()))
                {
                    return false;
                }
            }

            return true;
        }
        var declarations = _classDeclaration.MethodDeclarations()
            .Where(MethodSignatureCheck)
            .ToList();
        try
        {
            return new Type(methodCall.Scope(), declarations.First().ReturnTypeName());
        }
        catch (InvalidOperationException e)
        {
            throw new NoSuchMethodException(
                methodCall.MethodName().Value(), 
                methodCallArguments,
                _classDeclaration.ClassName().Value(), 
                e);
        }
    }

    public static bool ConstructorSignatureCheck(
        IConstructorDeclaration constructorDeclaration, 
        ITypesAwareArguments constructorCallArguments)
    {
        if (constructorDeclaration.Parameters().GetParameters().Count() != constructorCallArguments.Values().Count())
            return false;
        foreach (IParameterDeclaration parameter in constructorDeclaration.Parameters().GetParameters()) 
        {
            if (constructorCallArguments
                    .Values()
                    .Count(arg => 
                        arg.Type().TypeName().Value()
                            .Equals(
                                parameter.TypeName().Value())
                    ) != 1)
            {
                return false;
            }
        }

        return true;
    }

    public static bool ConstructorSignatureCheck(
        ITypesAwareConstructor constructorDeclaration, 
        ITypesAwareArguments constructorCallArguments)
    {
        if (constructorDeclaration.Parameters().GetParameters().Count() != constructorCallArguments.Values().Count())
            return false;
        foreach (ITypedParameter parameter in constructorDeclaration.Parameters().GetParameters()) 
        {
            if (constructorCallArguments
                    .Values()
                    .Count(arg => 
                        arg.Type().TypeName().Value()
                            .Equals(
                                parameter.Type().TypeName().Value())
                    ) != 1)
            {
                return false;
            }
        }

        return true;
    }

    public void ConstructorCallCheck(ITypesAwareArguments arguments)
    {
        var declarations = _classDeclaration.ConstructorDeclarations()
            .Where(constructor => ConstructorSignatureCheck(constructor, arguments))
            .ToList();
        try
        {
            declarations.First();
        }
        catch (InvalidOperationException e)
        {
            throw new NoSuchConstructorException(
                arguments,
                _classDeclaration.ClassName().Value(), 
                e);
        }
    }

    protected bool Equals(Type other)
    {
        return _classDeclaration.ClassName().Equals(other._classDeclaration.ClassName());
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
        return _classDeclaration.ClassName().GetHashCode();
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
        ITypesAwareArguments arguments,
        string className,
        InvalidOperationException invalidOperationException) :
        base("Method with name '" + 
             methodName + 
             "' and arguments with types {" + 
             String.Join("; ", arguments.Values().Select(arg => arg.Type().TypeName())) + 
             "} wasn't found in " + 
             className, 
            invalidOperationException)
    { }
}

public class NoSuchConstructorException : Exception
{
    public NoSuchConstructorException(
        ITypesAwareArguments arguments,
        string className,
        InvalidOperationException invalidOperationException) :
        base("Constructor of arguments with types {" + 
             String.Join("; ", arguments.Values().Select(arg => arg.Type().TypeName())) + 
             "} wasn't found in " + 
             className, 
            invalidOperationException)
    { }
}