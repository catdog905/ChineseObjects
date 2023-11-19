using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IMember : IAstNode {}

public interface IMembers : IAstNode
{
    public IEnumerable<IMember> GetMember();
}