namespace ChineseObjects.Lang;

public interface IScope
{
    T? EntityByName<T>(String name) where T : class;
}

public class EmptyScope : IScope
{
    public T? EntityByName<T>(String name) where T : class
    {
        return null;
    }
}