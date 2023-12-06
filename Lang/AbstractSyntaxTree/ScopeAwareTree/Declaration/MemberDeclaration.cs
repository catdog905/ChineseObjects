using System.Collections.Immutable;

namespace ChineseObjects.Lang.AbstractSyntaxTree.ScopeAwareTree.Declaration;

public interface IScopeAwareMember : IScopeAwareAstNode { }

public interface IScopeAwareMembers
{
    public IEnumerable<IScopeAwareMember> GetMemberDeclarations();
}

public class ScopeAwareMembers : IScopeAwareMembers
{
    private readonly Scope _scope;
    private readonly ImmutableList<IScopeAwareMember> _memberDeclarations;

    public ScopeAwareMembers(Scope scope, IEnumerable<IScopeAwareMember> memberDeclarations)
    {
        _scope = scope;
        _memberDeclarations = memberDeclarations.ToImmutableList();
    }

    public ScopeAwareMembers(Scope scope) : this(scope, ImmutableList<IScopeAwareMember>.Empty) {} 

    public ScopeAwareMembers(
        Scope scope,
        ScopeAwareMembers memberDeclarations,
        IScopeAwareMember memberDeclaration
    ) : this(scope, memberDeclarations._memberDeclarations.Add(memberDeclaration)) {}

    public ScopeAwareMembers(
        Scope scope,
        IScopeAwareMember memberDeclaration,
        ScopeAwareMembers memberDeclarations
    ) : this(scope, new[] {memberDeclaration}.Concat(memberDeclarations._memberDeclarations)) {}

    public Scope Scope()
    {
        return _scope;
    }

    public IEnumerable<IScopeAwareMember> GetMemberDeclarations()
    {
        return _memberDeclarations;
    }
}