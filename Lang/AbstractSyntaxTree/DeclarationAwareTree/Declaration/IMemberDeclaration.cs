using System.Collections.Immutable;

namespace ChineseObjects.Lang;

public interface IMemberDeclaration : IMember, IDeclarationAstNode {}

public class MemberDeclarations : IMembers, IDeclarationAstNode
{
    public readonly ImmutableList<IMemberDeclaration> MemberDeclarations_;

    public MemberDeclarations(IEnumerable<IMemberDeclaration> members) {
        MemberDeclarations_ = members.ToImmutableList();
    }

    public MemberDeclarations() : this(ImmutableList<IMemberDeclaration>.Empty) {}
    
    public MemberDeclarations(
        MemberDeclarations memberDeclarations,
        IMemberDeclaration memberDeclaration
    ) : this(memberDeclarations.MemberDeclarations_.Add(memberDeclaration)) {}

    public MemberDeclarations(
        IMemberDeclaration memberDeclaration,
        MemberDeclarations memberDeclarations
    ) : this(new[] {memberDeclaration}.Concat(memberDeclarations.MemberDeclarations_)) {}

    public override string ToString()
    {
        return String.Join(",", MemberDeclarations_);
    }

    public IEnumerable<IMember> GetMember()
    {
        return MemberDeclarations_;
    }
}