using System.Collections.Immutable;

namespace ChineseObjects.Lang.AbstractSyntaxTree.DeclarationAwareTree.Declaration;

public interface IMemberDeclaration : IAstNode {}

public interface IMemberDeclarations
{
    public IEnumerable<IMemberDeclaration> GetMemberDeclarations();
}

public class MemberDeclarations : IMemberDeclarations
{
    public readonly ImmutableList<IMemberDeclaration> _memberDeclarations;

    public MemberDeclarations(IEnumerable<IMemberDeclaration> members) {
        _memberDeclarations = members.ToImmutableList();
    }

    public MemberDeclarations() : this(ImmutableList<IMemberDeclaration>.Empty) {}
    
    public MemberDeclarations(
        MemberDeclarations memberDeclarations,
        IMemberDeclaration memberDeclaration
    ) : this(memberDeclarations._memberDeclarations.Add(memberDeclaration)) {}

    public MemberDeclarations(
        IMemberDeclaration memberDeclaration,
        MemberDeclarations memberDeclarations
    ) : this(new[] {memberDeclaration}.Concat(memberDeclarations._memberDeclarations)) {}

    public override string ToString()
    {
        return String.Join(",", _memberDeclarations);
    }

    public IEnumerable<IMemberDeclaration> GetMemberDeclarations()
    {
        return _memberDeclarations;
    }
}