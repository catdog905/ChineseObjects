namespace ChineseObjects.Lang;

public interface IParameters : IAstNode
{
    public IEnumerable<IParameter> GetParameters();
}