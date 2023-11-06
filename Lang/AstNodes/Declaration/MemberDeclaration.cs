using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public abstract class MemberDeclaration : IAstNode {}

public class MemberDeclarations
{
    public readonly ImmutableList<MemberDeclaration> MemberDeclarations_;

    public MemberDeclarations(IEnumerable<MemberDeclaration> members) {
        MemberDeclarations_ = members.ToImmutableList();
    }

    public MemberDeclarations() : this(ImmutableList<MemberDeclaration>.Empty) {}
    
    public MemberDeclarations(
        MemberDeclarations memberDeclarations,
        MemberDeclaration memberDeclaration
    ) : this(memberDeclarations.MemberDeclarations_.Add(memberDeclaration)) {}

    public MemberDeclarations(
        MemberDeclaration memberDeclaration,
        MemberDeclarations memberDeclarations
    ) : this(new[] {memberDeclaration}.Concat(memberDeclarations.MemberDeclarations_)) {}

    public override string ToString()
    {
        return String.Join(",", MemberDeclarations_);
    }
}